using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VL.Lib.Collections;

namespace SCSynth.SCNodes
{
    public enum ControlParameterType
    {
        Trigger,
        Value
    }

    public interface IControlParameter
    {
        object Value { get; }
        string Name { get; }
        public Action<IControlParameter> OnChanged { get; set; }
    }

    public class ObjectControParamer
    {
        public string Name;

        public object? InitValue;
        private object? _value;
        public object? Value
        {
            get => _value;
            set
            {


                if (value is float || value is int)
                {
                    if (!_value.Equals(value))
                    {
                        _value = value;
                        //OnChanged?.Invoke(this);
                        Console.WriteLine("Parameter Changed");
                    }


                }
                if (value is Spread<float> || value is Spread<IComparable>)
                {
                    if (!ReferenceEquals(_value, value))
                    {
                        _value = value;
                        Console.WriteLine("Parameter Sequence Changed");
                    }
                }

            }
        }

        public ObjectControParamer(string name, object initValue)
        {
            this.Name = name;
            this.InitValue = initValue; 
            this._value = initValue;
        }
    }

    public class ControlParameter
    {
        public ControlParameterType Type { private set; get; }
        public string Name;
        
        public float InitValue;
        private float _value;
        public float Value
        {
            get =>_value;
            // This is the trigger: when the value is set (via vvvv UI or logic), 
            // it fires the OnChanged action.
            set
            {
                
                    
                    if (_value != value)
                    {
                        _value = value;
                        OnChanged?.Invoke(this);
                        //Console.WriteLine("Parameter Changed");
                    }
                
                
            }
        }

        
        // This action acts as the bridge to the Synth class
        public Action<ControlParameter> OnChanged;
        public ControlParameter(ControlParameterType type, string name, float initValue)
        {
            this.Type = type;
            Name = name;
            Value = InitValue = initValue;
            
         
        }
        

    }


    public class ValueControlParameter : ControlParameter
    {
        public ValueControlParameter(string name, float initValue) : base(ControlParameterType.Value, name, initValue)
        {
        }
    }

    public class TriggerControlParameter : ControlParameter
    {
        
        public TriggerControlParameter(string name, bool initValue) : base(ControlParameterType.Trigger, name, initValue == true ? 1 : 0)
        {
            
        }

        public void SetValue(bool Value)
        {
            this.Value = Value==true?1:0;
        }
       
    }



}
