using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Schedular.API.Models;
using Schedular.API.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using Schedular.API.Helpers;

namespace Schedular.API.Data
{
    public class TaskScheduleRepository : ITaskScheduleRepository
    {
        //access to the database
        private readonly DataContext _context;
        public TaskScheduleRepository(DataContext context)
        {
            _context = context;
        }

        //add new data to the database
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }
        
        // update database
        public TaskSchedule Update(int Id, TaskSchedule taskSchedule) 
        {
            var TaskScheduleDb =  _context.TaskSchedules.SingleOrDefault(s => s.Id == Id);

            TaskScheduleDb.Title = taskSchedule.Title;
            TaskScheduleDb.Start = taskSchedule.Start;
            TaskScheduleDb.End = taskSchedule.End;
            TaskScheduleDb.userCurrentAssignedId = taskSchedule.userCurrentAssignedId;
            TaskScheduleDb.userLastEditId = taskSchedule.userLastEditId;
            TaskScheduleDb.highPriority = taskSchedule.highPriority;
            TaskScheduleDb.isClosed = taskSchedule.isClosed;
            TaskScheduleDb.hasTimeLimit = taskSchedule.hasTimeLimit;
            TaskScheduleDb.Attachments = taskSchedule.Attachments;
            TaskScheduleDb.customerId = taskSchedule.customerId;
            TaskScheduleDb.userLastEditDate = taskSchedule.userLastEditDate;
            

            _context.SaveChanges();

            return TaskScheduleDb;
        }    

        // Delete data

        public void Delete(int id)
        {
            _context.TaskSchedules.Remove(_context.TaskSchedules
                .FirstOrDefault(t => t.Id == id));
            _context.SaveChanges();        
        }

        //get individual taskschedules data
        public async Task<IList<TaskSchedule>> GetTask(int id)
        {    
             var taskSchedule = await _context.TaskSchedules
                .Where(u => u.Id == id)
                .Include(ts => ts.Notes)
                .Include(ts => ts.Attachments)
                .Include(c => c.customer)
                .ToListAsync();

                taskSchedule.ForEach(t => t.Notes = t.Notes.OrderByDescending(n => n.DateCreated).ToList());
                
            return taskSchedule;  
        
        }  
     

        //get all data of taskSchedules
        public async Task<IEnumerable<TaskSchedule>> GetTasks()
        {
            var taskSchedule = await _context.TaskSchedules
                .Include(ts => ts.Notes)
                .Include(c => c.customer)
                .OrderByDescending(x => x.taskCreatedDate)
                .ToListAsync();             
           
           return taskSchedule;
        }
        
    
        //get users tasks
        public async Task<PagedList<TaskSchedule>> GetOpenCloseTasksByUser(int userId, bool isClosed, TaskParams taskParams)
        {
            if(isClosed == true) 
            {
                var query = _context.TaskSchedules
                    .Where(u => u.userCurrentAssignedId == userId)
                    .Where(c =>c.isClosed == true)
                    .Include(c => c.customer)
                    .OrderByDescending(x => x.taskCreatedDate)
                    .AsNoTracking();

                //gets sent to the pagination methods to be paginated 
                return await PagedList<TaskSchedule>
                    .CreateAsync(query, taskParams.Pagenumber, taskParams.PageSize);
            }
            else 
            {
                var query = _context.TaskSchedules
                    .Where(u => u.userCurrentAssignedId == userId)
                    .Where(c =>c.isClosed == false) 
                    .Include(c => c.customer)
                    .OrderByDescending(x => x.taskCreatedDate)
                    .AsNoTracking();
                
                //gets sent to the pagination methods to be paginated 
                return await PagedList<TaskSchedule>
                    .CreateAsync(query, taskParams.Pagenumber, taskParams.PageSize);
            }                             
        }

        
        //get users tasks
        public async Task<PagedList<TaskSchedule>> GetHighPriorityOpenCloseTasksByUser(int userId, bool isClosed, bool HighPriority, TaskParams taskParams)
        {
            if(isClosed == false && HighPriority == true) //open high priority
            {
                var query = _context.TaskSchedules
                    .Where(u => u.userCurrentAssignedId == userId)
                    .Where(c => c.isClosed == false)
                    .Where(h => h.highPriority == true)
                    .Include(c => c.customer)
                    .OrderByDescending(x => x.taskCreatedDate)
                    .AsNoTracking();

                //gets sent to the pagination methods to be paginated 
                return await PagedList<TaskSchedule>
                    .CreateAsync(query, taskParams.Pagenumber, taskParams.PageSize);
            }
            else if(isClosed == true && HighPriority == true) //closed high priority
            {
                var query = _context.TaskSchedules
                    .Where(u => u.userCurrentAssignedId == userId)
                    .Where(c =>c.isClosed == true)
                    .Where(h => h.highPriority == true) 
                    .Include(c => c.customer)
                    .OrderByDescending(x => x.taskCreatedDate)
                    .AsNoTracking();
                
                //gets sent to the pagination methods to be paginated 
                return await PagedList<TaskSchedule>
                    .CreateAsync(query, taskParams.Pagenumber, taskParams.PageSize);
            } 
            else if(isClosed == false && HighPriority == false) //open normal priority
            {
                var query = _context.TaskSchedules
                    .Where(u => u.userCurrentAssignedId == userId)
                    .Where(c =>c.isClosed == false)
                    .Where(h => h.highPriority == false) 
                    .Include(c => c.customer)
                    .OrderByDescending(x => x.taskCreatedDate)
                    .AsNoTracking();
                
                //gets sent to the pagination methods to be paginated 
                return await PagedList<TaskSchedule>
                    .CreateAsync(query, taskParams.Pagenumber, taskParams.PageSize);
            } 
            else // closed normal priority 
            {
               var query = _context.TaskSchedules
                    .Where(u => u.userCurrentAssignedId == userId)
                    .Where(c =>c.isClosed == true)
                    .Where(h => h.highPriority == false) 
                    .Include(c => c.customer)
                    .OrderByDescending(x => x.taskCreatedDate)
                    .AsNoTracking();
                
                //gets sent to the pagination methods to be paginated 
                return await PagedList<TaskSchedule>
                    .CreateAsync(query, taskParams.Pagenumber, taskParams.PageSize);
            }                              
        }

        
        //get users with notes test 
        public async Task<PagedList<TaskSchedule>> GetTaskSchedulesByUser(int UserCurrentAssignedId, TaskParams taskParams)
        {
               var query = _context.TaskSchedules
                    .Include(ts => ts.Notes)
                    .Include(c => c.customer)
                    .Where(u => u.userCurrentAssignedId == UserCurrentAssignedId)
                    .OrderByDescending(x => x.taskCreatedDate)
                    .AsNoTracking();

                //gets sent to the pagination methods to be paginated 
                return await PagedList<TaskSchedule>
                    .CreateAsync(query, taskParams.Pagenumber, taskParams.PageSize);         
        }

         //get tasks user for calendar  
        public async Task<List<TaskSchedule>> GetTaskSchedulesByUserCalendar(int UserCurrentAssignedId)
        {
            var query =  await _context.TaskSchedules
                .Include(ts => ts.Notes)
                .Include(c => c.customer)
                .Where(u => u.userCurrentAssignedId == UserCurrentAssignedId)
                .Where(a => a.isClosed == false)
                .ToListAsync();   

            return query;  
        }


        //lets us know if changes have been saved on the database
        //if true is returned then changes have been made
        //if false is returned then no changes have been saved
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
       
        public Task<IEnumerable<TaskSchedule>> GetOpenCloseTasksByUser(int userId)
        {
            throw new NotImplementedException();
        }
    }
}