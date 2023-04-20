using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DateTimeController : ControllerBase
    {
        private readonly DataContext _context;
        public DateTimeController(DataContext context)
        {
            _context = context;
            
        }

        [HttpGet]
        [Route("/datetime/{id}")]
        public IActionResult GetDateTime(int id, string timezoneId)
        {
            
            DateTime utcDateTime = DateTime.UtcNow; 
            TimeZoneInfo userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId); 
            DateTime userLocalDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, userTimeZone); 
            return Ok(userLocalDateTime);
        }

        [Route("/datetime")]
        [HttpPost]
        public ActionResult SaveDateTime([FromBody] DateTimeModel model)
        {
            try
            {
                DateTime utcDateTime = model.Value;

                TimeZoneInfo selectedTimeZone;
                if (model.Id >= -12 && model.Id <= 14) // valid offset range
                {
                    // Create custom time zone using offset
                    TimeSpan offset = new TimeSpan(model.Id, 0, 0);
                    selectedTimeZone = TimeZoneInfo.CreateCustomTimeZone($"CustomTimezone_{model.Id}", offset, $"CustomTimezone_{model.Id}", $"CustomTimezone_{model.Id}");
                }
                else
                {
                    // Use model.Id as time zone ID
                    selectedTimeZone = TimeZoneInfo.FindSystemTimeZoneById(model.Id.ToString());
                }

                DateTime convertedDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, selectedTimeZone);

                DateTimeValue dateTimeValue = new DateTimeValue
                {
                    ConvertedDateTime = convertedDateTime
                };

                _context.DateTimeValues.Add(dateTimeValue);
                _context.SaveChanges();

                return Ok("Datetime value saved successfully in " + selectedTimeZone.DisplayName + " timezone");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        /*[Route("/datetime")]
        [HttpPost]
        public ActionResult SaveDateTime([FromBody] DateTimeModel model)
        {
           try
            {
                DateTime utcDateTime = model.Value; 

                string selectedTimezoneId = (model.Id).ToString(); 
                TimeZoneInfo selectedTimeZone = TimeZoneInfo.FindSystemTimeZoneById(selectedTimezoneId);
                DateTime convertedDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, selectedTimeZone);

                DateTimeValue dateTimeValue = new DateTimeValue
                {
                    ConvertedDateTime = convertedDateTime
                };

                _context.DateTimeValues.Add(dateTimeValue);

                _context.SaveChanges();

                return Ok("Datetime value saved successfully in " + selectedTimeZone.DisplayName + " timezone");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        } */

    }
}