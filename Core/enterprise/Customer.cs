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
            get => _orders.Where(x => x.IsIncomplete)
                .Select(x => x.ToString())
                .ToList();
        }

        public List<string> CompleteOrders {
            get => _orders.Where(x => x.IsComplete)
                .Select(x => x.ToString())
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
            _orders.First(x => x.Type == type && x.IsIncomplete)
                .CompletePo(dayTime);
        }

        public void Work(DayTime dayTime)
        {
            if (_enterprise == null) { return; }

            var ordersToSend = _orders.Where(x => x.Sent == false);
            foreach(var order in ordersToSend)
            {
                _enterprise.StartOrder(order.Type, order.Due);
                order.MarkAsSent();
            }
        }

        private class Order
        {
// Properties
            public DayTime Due { get; }
            public DayTime Complete { get; private set;}
            public bool IsComplete { get => Complete != null; }
            public bool IsIncomplete { get => Complete == null; }
            public bool Sent { get; private set; }
            public string Type { get; }

// Constructor
            public Order(string type, DayTime due)
            {
                Type = type;
                Due = due;
                Complete = null;
                Sent = false;
            }

// Pure Methods
            public string CompleteString() 
            { 
                if (IsIncomplete) { return ""; }
                return ConvertTime(Complete);
            }

            public string DueString() { return ConvertTime(Due); }

            public override string ToString()
            {
                const string deliminator = " ; ";
                string answer = Type + deliminator + ConvertTime(Due);
                if(IsComplete)
                {
                    answer += deliminator + ConvertTime(Complete);
                }
                return answer;
            }

// Impure Methods
            public void CompletePo(DayTime dayTime)
            {
                Complete = dayTime;
            }

            public void MarkAsSent()
            {
                Sent = true;
            }

// Private Methods
            private string ConvertTime(DayTime time)
            {
                return time.Day + ":" + time.Time;
            }
        }
    }
}