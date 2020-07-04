namespace Core.Resources
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;

  public interface ICustomQueue
  {
    int Count { get; }

    bool Any();
    IWork Dequeue();
    bool Empty();
    void Enqueue(IWork wo);
    IWork Find(int id);
    int? FirstId();
    IEnumerator<IWork> GetEnumerator();
    int? LastId();
    IWork Remove(int id);
  }

  public class NeoQueue : ICustomQueue, IEnumerable<IWork>
  {
    private readonly Dictionary<int, IWork> _dict;
    private readonly Dictionary<int, int> _wo_order;
    private readonly Dictionary<int, int> _order_wo;

    private int _min;
    private int _max;

    public NeoQueue()
    {
      _dict = new Dictionary<int, IWork>();
      _wo_order = new Dictionary<int, int>();
      _order_wo = new Dictionary<int, int>();
      _min = 0;
      _max = 0;
    }

    public int Count { get => _dict.Count; }
    public Dictionary<int, IWork>.ValueCollection Values { get => _dict.Values; }

    public bool Any()
    {
      return _min > 0;
    }

    public IWork Dequeue()
    {
      if(_dict.Count == 0){ throw new System.IndexOutOfRangeException("No IWork Items"); }
      int woId = _order_wo[_min];
      IWork answer = _dict[woId];

      _dict.Remove(woId);
      _wo_order.Remove(woId);
      _order_wo.Remove(_min);

      ConsolidateOrdering();

      return answer;
    }

    public bool Empty()
    {
      return _min == 0;
    }

    public void Enqueue(IWork wo)
    {
      _dict.Add(wo.Id, wo);
      _max++;
      _wo_order.Add(wo.Id, _max);
      _order_wo.Add(_max, wo.Id);

      if(_min == 0){ _min = _max; }
    }

    public IWork Find(int id)
    {
      return _dict[id];
    }

    public int? FirstId()
    {
      if (_min == 0) { return null; }
      return _order_wo[_min];
    }

    public IEnumerator<IWork> GetEnumerator()
    {
      foreach (int order in _order_wo.Keys)
      {
        int woid = _order_wo[order];
        yield return _dict[woid];
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public int? LastId()
    {
      if (_max == 0) { return null; }
      return _order_wo[_max];
    }

    public IWork Remove(int woId)
    {
      int order = _wo_order[woId];
      IWork answer = _dict[woId];

      _dict.Remove(woId);
      _wo_order.Remove(woId);
      _order_wo.Remove(order);

      ConsolidateOrdering();

      return answer;
    }

    private void ConsolidateOrdering()
    {
      if(_order_wo.Count == 0)
      {
        _min = 0;
        _max = 0;
        return;
      }

      while(!_order_wo.ContainsKey(_min)) {
        _min++;
        if (_min > _max )
        {
          throw new System.IndexOutOfRangeException("NeoQueue Min > Max !!");
        }
      }

      if(_order_wo.Count == 1) {
        _max = _min;
        return;
      }

      while(!_order_wo.ContainsKey(_max)) {
        _max--;
        if (_max < _min)
        {
          throw new System.IndexOutOfRangeException("NeoQueue Max < Min !!");
        }
      }

      return;
    }
  }
}