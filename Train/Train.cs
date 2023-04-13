using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    enum TrainType
    {
        Passenger,
        Freight,
    }

    internal class Train
    {
        private Dictionary<uint,Carriage> carriages;
        private DateTime arrival;

        public DateTime Dispatch { get; set; }
        public string Route { get; set; }
        public uint Number { get; init; }
        public TrainType Type { get; init; }
        public TimeSpan TimeToArrival { get { return arrival - DateTime.Now; } }
        public int CarriageCount { get { return carriages.Count; } }
        public DateTime Arrival
        {
            get { return arrival; }
            set 
            {
                if (arrival < Dispatch) throw new Exception($" Error in the arrival time of train number \"{Number}\"");
                else arrival =  value; 
            }
        }
        public float AvrPass 
        {
            get 
            {
                uint average = 0;
                foreach (var car in carriages)
                    average += car.Value.Passenger;
                return (float)Math.Round((float)average/carriages.Count,2);
            }
        }
        public uint FreeSeatsCount
        {
            get
            {
                uint count = 0;
                foreach (var car in carriages)
                    count += car.Value.FreeSeats;
                return count;
            } 
        }

        public Train(uint trainNumber,TrainType type,string route,DateTime dispatch,DateTime arrival,params Carriage[] carriages)
        {
            this.carriages = new  Dictionary<uint, Carriage>();
            Route = route;
            Number = trainNumber;
            Type = type;
            Dispatch = dispatch;
            this.arrival = arrival;
            for (int i = 0; i < carriages.Length; i++)
                if (!this.carriages.TryAdd(carriages[i].Number, carriages[i])) 
                    throw new Exception($" Dublicate carriage number {carriages[i].Number}");
        }

        public override string ToString() 
        {
            StringBuilder sb = new StringBuilder($" -= Train \"{Number}\" =-\n");
            sb.AppendLine($" Route        : {Route}");
            sb.AppendLine($" Dispatch     : {Dispatch}");
            sb.AppendLine($" Arrival      : {arrival}");
            sb.Append(" Arrival via  : "); 
            if (TimeToArrival.TotalHours < 0) sb.AppendLine("arrived");
            else sb.AppendLine($"{TimeToArrival.Hours}:{TimeToArrival.Minutes}");
            sb.AppendLine($" Type         : {Type}");
            sb.AppendLine($" Carriages    : {carriages.Count}");
            sb.AppendLine($" Free seats   : {FreeSeatsCount}");
            sb.AppendLine($" Average pass.: {AvrPass}\n");
            sb.AppendLine($" ----- Carriages info -----\n");
            foreach (var car in carriages)
                sb.Append(car.Value.ToString());
            return sb.ToString();
        }

        public int AddCarriage( in Carriage carriage)
        {
            if (!carriages.TryAdd(carriage.Number, carriage)) throw new Exception($" Dublicate carriage number {carriage.Number}");
            return carriages.Count;
        }

        public int AddCarriage(uint number, CType type, uint seats, uint passengers = 0)
        {
             return AddCarriage(new Carriage(number, type, seats, passengers));
        }

        public void PassBoarding(uint carriageNumber, ref uint passCount)
        {
            
            if (!carriages.TryGetValue(carriageNumber, out Carriage tmp)) throw new Exception($" There is no carriage with number {carriageNumber}");
            if (passCount == 0) return;
            else if (passCount > tmp.FreeSeats)
            {
                passCount -= tmp.FreeSeats;
                tmp.Passenger += tmp.FreeSeats;
            }
            else
            {
                tmp.Passenger += passCount;
                passCount = 0;
            }
            carriages[carriageNumber] = tmp;
        }

        public void PassBoarding(ref uint passCount)
        {
            foreach (var car in carriages)
            {
                if (passCount == 0) break;
                PassBoarding(car.Value.Number, ref passCount);
            }
        }

            public void PassDisembarkation(uint carriageNumber,  uint passCount = 0)
        {
            if (!carriages.TryGetValue(carriageNumber,out Carriage tmp)) throw new Exception($" There is no carriage with number {carriageNumber}");
            if (passCount == 0 || passCount > tmp.Passenger) tmp.Passenger = 0;
            else tmp.Passenger -= passCount;
            carriages[carriageNumber] = tmp;    
        }
    }
}
