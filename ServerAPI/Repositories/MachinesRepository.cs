using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClientMachineManager.Domain;

namespace ServerAPI.Repositories
{
    public static class MachinesRepository
    {
        public static void Add(MachineInfo machineInfo)
        {
            using (var db = new LiteDatabase(@"C:\Temp\MyData.db"))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<MachineInfo>("machines");

                if (col.Query().Where(m => m.LocalIP == machineInfo.LocalIP).Count() == 0)
                {
                    // Insert new customer document (Id will be auto-incremented)
                    col.Insert(machineInfo);
                }
            }
        }

        public static List<MachineInfo> GetAll()
        {
            List<MachineInfo> result = null;
            using (var db = new LiteDatabase(@"C:\Temp\MyData.db"))
            {
                var col = db.GetCollection<MachineInfo>("machines");

                result = col.Query().ToList();
            }

            return result;
        }
    }
}
