using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Schedular.API.Data;
using Schedular.API.Models;


namespace Schedular.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController] 
    public class TaskScheduleController : ControllerBase
    {   
        //initialise the taskschedule repository    
        private readonly ITaskScheduleRepository _repo;

        // add DTO code here 
        //contructor to use private _repo
        public TaskScheduleController(ITaskScheduleRepository repo)
        {
            _repo = repo;
            // add DTO code here 
        }

        [HttpGet]
        public async Task<IActionResult> GetTaskSchedule()
        {
            var taskschedule = await _repo.GetTasks();

            // add DTO code here 

            return Ok(taskschedule);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserTaskSchedule(int id)
        {
            var taskUserSchedule = await _repo.GetTaskSchedulesByStaffId(id);
            
            return Ok(taskUserSchedule);
            
        }

        [HttpPost]        
        public async Task<IActionResult> PostSchedule(TaskSchedule taskSchedule)
        {
             _repo.Add(taskSchedule);  

            //save to the database
            if(await _repo.SaveAll())
                return Ok();   
            return BadRequest("Failed to save Schedule");   
        }
        [HttpPut("{id}")]
        public TaskSchedule PutSchedule(int id, [FromBody] TaskSchedule taskSchedule)
        {
            TaskSchedule taskSchedulePut = _repo.Update(id, taskSchedule);    

            return taskSchedulePut;      
        }
        [HttpDelete("{id}")]
        public void DeleteTaskSchedule(int id)
        {
            _repo.Delete(id);
        }
    }    
}