using App9databind.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App9databind.Models
{
    class Organization
    {
        public List<App9databind.Data.clsDogs> dogs { get; set; }

        public Organization()
        {
            dogs = service.GetData();
        }

        public void Add(clsDogs dog)
        {
            if (!dogs.Contains(dog))
            {
                dogs.Add(dog);
                service.Write(dog);
            }
        }

        public void Delete(clsDogs dog)
        {
            if (dogs.Contains(dog))
            {
                dogs.Remove(dog);
                service.Delete(dog);
            }
        }

        public void Update(clsDogs dog)
        {
            service.Write(dog);
        }
    }
}
