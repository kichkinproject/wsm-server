using Microsoft.EntityFrameworkCore;
using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DBModel.DBContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Repositories
{
    public class UserGroupRepository
    {
        private readonly MainDbContext _context;
        public UserGroupRepository(MainDbContext context)
        {
            _context = context;
        }

        public DbSet<UserGroup> UserGroups { get => _context.UserGroups; }

        public HashSet<long> GetParentGroups(long id)
        {
            var input = UserGroups.ToDictionary(x => x.Id, x => x);
            var returningAnswer = new HashSet<long>();
            if(input.ContainsKey(id))
            {
                var cur = input[id];
                while (cur.ParentGroupId.HasValue)
                {
                    returningAnswer.Add(cur.Id);
                    cur = input[cur.ParentGroupId.Value];
                }
                returningAnswer.Add(cur.Id);
                return returningAnswer;
            }
            else
            {
                throw new Exception("Группа с таким Id не найдена");
            }
        }
        public HashSet<long> GetChildrenGroups(long id)
        {
            var children = new Dictionary<long, List<long>>();
            foreach(var userGroup in UserGroups)
            {
                if(userGroup.ParentGroupId.HasValue)
                {
                    if (children.ContainsKey(userGroup.ParentGroupId.Value))
                        children[userGroup.ParentGroupId.Value].Add(userGroup.Id);
                    else
                        children.Add(userGroup.ParentGroupId.Value, new List<long> { userGroup.Id });
                }
            }

            var returningAnswer = new HashSet<long>();

            returningAnswer.Add(id);
            var queue = new Queue<long>();
            queue.Append(id);

            while(queue.Count>0)
            {
                var current = queue.Dequeue();
                if (children.ContainsKey(current))
                    foreach (var a in children[current])
                    {
                        returningAnswer.Add(a);
                        queue.Append(a);
                    }
            }


            return returningAnswer;
        }
        public UserGroup this[long id]
        {
            get { return UserGroups.Where(x => x.Id == id).FirstOrDefault(); }
        }

        public UserGroup Create(UserGroup input)
        {
            _context.Add(input);
            return input;
        }
        public void Edit(UserGroup input)
        {
            var userGroup = UserGroups.Where(x => x.Id == input.Id).FirstOrDefault();
            if (userGroup == null)
                throw new Exception("Группа с таким Id не найдена");
            userGroup.Name = input.Name;
        }
        public void Delete(long id)
        {
            var userGroup = UserGroups.Where(x => x.Id == id).FirstOrDefault();
            if (userGroup == null)
                throw new Exception("Группа с таким Id не найдена");
            _context.Remove(userGroup);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
