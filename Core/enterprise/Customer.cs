namespace Core.Enterprise
{
    using System.Collections.Generic;
    using System.Linq;

    public interface IRequestWork
    {
        List<string> ActiveOrders { get; }
        List<string> CompleteOrders { get; }
        void AddEnterprise(IEnterprise enterprise);
        void CreateOrder(string po_type, DayTime due);
        void ReceiveProduct(string type, DayTime dayTime);
        void Work(DayTime dayTime);
    }

    public class Customer : IRequestWork
    {
        private IEnterprise _enterprise;
        private List<Order> _orders;

        public Customer()
        {
            _enterprise = null;
            _orders = new List<Order>();
        }

        public List<string> ActiveOrders {
            get => _orders.Where(x => x.Complete == null)
                .Select(x => x.Type + " ; " + x.DueString())
                .ToList();
        }

        public List<string> CompleteOrders {
            get => _orders.Where(x => x.Complete != null)
                .Select(x => x.Type + " ; " + x.DueString() + " ; " + x.CompleteString())
                .ToList();
        }

        public void AddEnterprise(IEnterprise enterprise)
        {
            if(_enterprise != null) { return; }
            _enterprise = enterprise;
        }

        public void CreateOrder(string po, DayTime due)
        {
            _orders.Add(new Order(po, due));
        }

        public void ReceiveProduct(string type, DayTime dayTime)
        {
            _orders.First(x => x.Type == type && x.Complete == null)
                .CompletePo(dayTime);
        }

        public void Work(DayTime dayTime)
        {
            var ordersToSend = _orders.Where(x => x.Sent == false);
            foreach(var order in ordersToSend)
            {
                _enterprise?.CreateOrder(order.Type, order.Due);
                order.Send();
            }
        }

        private class Order
        {
            public string Type { get; }
            public DayTime Due { get; }
            public DayTime Complete { get; private set;}
            public bool Sent { get; private set; }

            public Order(string type, DayTime due)
            {
                Type = type;
                Due = due;
                Complete = null;
                Sent = false;
            }

            public override string ToString()
            {
                string answer = Type + " ; " + ConvertTime(Due);
                if(Complete != null)
                {
                    answer += " ; " + ConvertTime(Complete);
                }
                return answer;
            }

            public void CompletePo(DayTime dayTime)
            {
                Complete = dayTime;
            }

            public string DueString() { return ConvertTime(Due); }
            public string CompleteString() 
            { 
                if (Complete == null) { return ""; }
                return ConvertTime(Complete);
            }

            public void Send()
            {
                Sent = true;
            }

            private string ConvertTime(DayTime time)
            {
                return time.Day + ":" + time.Time;
            }
        }
    }
}