
using Inside.Models;
using Inside.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inside.Controllers
{
    [Route("api/public")]
    public class InsidePublicTest : ControllerBase
    {

        private readonly InsideDbContext _context;

        public InsidePublicTest(InsideDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("messages")]
        public async Task<ActionResult<List<Message>>> GetMessages()
        {
            return Ok(await _context.Messages.ToListAsync());
        }

        [HttpPost]
        [Route("message")]
        public async Task<ActionResult<List<MessageResponceModel>>> PostMessage([FromBody] PostMessageModel dto)
        {
            User? currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if(currentUser != null)
            {
                if (dto.TextOfMessage == "history 10")
                {
                    var messageCount = _context.Messages.Count();
                    var mes = _context.Messages.ToList();
                    if (messageCount >= 10) 
                    {
                        #region eager return
                        //// Eager return - плохо читабельный, но с циркулярными связями. Возвращаемый тип <List<Message>>
                        //return Ok(mes.GetRange(messageCount - 10, 10));
                        #endregion 

                        #region MessageResponceModel return
                        // возвращаем пользоватлю только сообщени, зачем ему этот мусор. Lazy return
                        var range = mes.GetRange(messageCount - 10, 10);
                        var messages = new List<MessageResponceModel>();
                        foreach(var message in range)
                        {
                            messages.Add(new MessageResponceModel
                            {
                                Username = message.Username,
                                TextOfTheMessage = message.TextOfMessage
                            }); ;
                        }
                        return Ok(messages);
                        #endregion

                    }
                    return Ok(mes);
                    
                }
                _context.Messages.Add(new Inside.Data.Message
                {
                    Username = "Guest",
                    TextOfMessage = dto.TextOfMessage
                });
                await _context.SaveChangesAsync();
                return Created("Message was added to Data Base", dto.TextOfMessage);
            }
            else
            {
                return BadRequest($"\"{dto.Username}\" not registered in the system. Your request does not handled");
            }       

        }
    }
}
