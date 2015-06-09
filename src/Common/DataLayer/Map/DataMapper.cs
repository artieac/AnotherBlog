using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace AlwaysMoveForward.Common.DataLayer.Map
{
    public class DataMapper<DomainClass, DTOClass, CommonInterface> where DomainClass : class, CommonInterface, new() where DTOClass : class, CommonInterface, new()
    {
        private static DataMapper<DomainClass, DTOClass, CommonInterface> mapperInstance;

        public static DataMapper<DomainClass, DTOClass, CommonInterface> GetInstance()
        {
            if (mapperInstance == null)
            {
                mapperInstance = new DataMapper<DomainClass, DTOClass, CommonInterface>();
            }

            return mapperInstance;
        }

        public DataMapper() { }

        public DomainClass Map(DTOClass source)
        {
            DomainClass retVal = null;
            
            if (source != null)
            {
                retVal = new DomainClass();

                if (retVal is CommonInterface && source is CommonInterface)
                {
                    PropertyInfo[] interfaceProperties = typeof(CommonInterface).GetProperties();

                    for (int i = 0; i < interfaceProperties.Count(); i++)
                    {
                        interfaceProperties[i].SetValue(retVal, interfaceProperties[i].GetValue(source, null), null);
                    }
                }
            }

            return retVal;
        }

        public IList<DomainClass> Map(IList<DTOClass> source)
        {
            IList<DomainClass> retVal = new List<DomainClass>();

            if (source != null)
            {
                for (int i = 0; i < source.Count(); i++)
                {
                    retVal.Add(this.Map(source[i]));
                }
            }

            return retVal;
        }

        public DTOClass Map(DomainClass source)
        {
            DTOClass retVal = null;

            if (source != null)
            {
                retVal = new DTOClass();

                if (retVal is CommonInterface && source is CommonInterface)
                {
                    PropertyInfo[] interfaceProperties = typeof(CommonInterface).GetProperties();

                    for (int i = 0; i < interfaceProperties.Count(); i++)
                    {
                        interfaceProperties[i].SetValue(retVal, interfaceProperties[i].GetValue(source, null), null);
                    }
                }
            }

            return retVal;
        }

        public DTOClass MapCopy(DomainClass source, DTOClass destination, string valueToExclude)
        {
            if (source != null && destination != null)
            {
                if (destination is CommonInterface && source is CommonInterface)
                {
                    PropertyInfo[] interfaceProperties = typeof(CommonInterface).GetProperties();

                    for (int i = 0; i < interfaceProperties.Count(); i++)
                    {
                        interfaceProperties[i].SetValue(destination, interfaceProperties[i].GetValue(source, null), null);
                    }
                }
            }

            return destination;
        }

        public IList<DTOClass> Map(IList<DomainClass> source)
        {
            IList<DTOClass> retVal = new List<DTOClass>();

            if (source != null)
            {
                for (int i = 0; i < source.Count(); i++)
                {
                    retVal.Add(this.Map(source[i]));
                }
            }

            return retVal;
        }

        public CommonInterface IMap(DTOClass source)
        {
            DomainClass retVal = null;

            if (source != null)
            {
                retVal = new DomainClass();

                if (retVal is CommonInterface && source is CommonInterface)
                {
                    PropertyInfo[] interfaceProperties = typeof(CommonInterface).GetProperties();

                    for (int i = 0; i < interfaceProperties.Count(); i++)
                    {
                        interfaceProperties[i].SetValue(retVal, interfaceProperties[i].GetValue(source, null), null);
                    }
                }
            }

            return retVal;
        }

        public IList<CommonInterface> IMap(IList<DTOClass> source)
        {
            IList<CommonInterface> retVal = new List<CommonInterface>();

            if (source != null)
            {
                for (int i = 0; i < source.Count(); i++)
                {
                    retVal.Add(this.Map(source[i]));
                }
            }

            return retVal;
        }

        public CommonInterface IMap(DomainClass source)
        {
            DTOClass retVal = null;

            if (source != null)
            {
                retVal = new DTOClass();

                if (retVal is CommonInterface && source is CommonInterface)
                {
                    PropertyInfo[] interfaceProperties = typeof(CommonInterface).GetProperties();

                    for (int i = 0; i < interfaceProperties.Count(); i++)
                    {
                        interfaceProperties[i].SetValue(retVal, interfaceProperties[i].GetValue(source, null), null);
                    }
                }
            }

            return retVal;
        }

        public IList<CommonInterface> IMap(IList<DomainClass> source)
        {
            IList<CommonInterface> retVal = new List<CommonInterface>();

            if (source != null)
            {
                for (int i = 0; i < source.Count(); i++)
                {
                    retVal.Add(this.Map(source[i]));
                }
            }

            return retVal;
        }     
    }
}
