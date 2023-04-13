﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    enum CType
    {
        Reserved,
        Freight,
        Сompartment
    }
    class Carriage : IComparable<Carriage>, ICloneable, IEquatable<Carriage>
    {
        private uint passengers;
        
        public uint Seats { get; init; }
        public CType Type { get; init; }
        public uint Number { get; init; }
        public uint FreeSeats { get { return Seats - passengers; } }

        public uint Passenger
        {
            get => passengers;
            set
            {
                if (value > Seats) throw new Exception($" Invalid passengers count of carriage number \"{Number}\"");
                else passengers = value;
            }
        }

        public Carriage(uint number, CType type, uint seats, uint passengers = 0)
        {
            Number = number;   
            Type = type;   
            Seats = seats; 
            this.Passenger = passengers; 
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder($"Сarriage \"{Number}\"\n");
            sb.AppendLine($" Type       : {Type}");
            sb.AppendLine($" Seats      : {Seats}");
            sb.AppendLine($" Passengers : {passengers}");
            sb.AppendLine($" Free seats : {FreeSeats}\n");
            return sb.ToString();
        }

        public int CompareTo(Carriage? other) => Type.CompareTo(other?.Type );

        public object Clone() => (Carriage)this.MemberwiseClone();

        public override bool Equals(object? obj) => obj is Carriage carriage && Equals(carriage);
        public override int GetHashCode() => HashCode.Combine( Type, Number);
        public bool Equals(Carriage? other)
        {
            return other != null &&
                   Type == other.Type &&
                   Number == other.Number;
        }
    }
}
