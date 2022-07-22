using Inside.Data;
using Inside.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Inside.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography.X509Certificates;

namespace Inside.Controllers
{
    [Authorize]
    [Route("api/inside")]
    public class InsideAuth : ControllerBase
    {
        private readonly InsideDbContext _context;
        private readonly JwtWorker _jwtWorker;
              

        public InsideAuth (InsideDbContext context,
                             JwtWorker jwtWorker)
        {
            _context = context;
            _jwtWorker = jwtWorker;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]        
        public async Task<ActionResult<string>> Login([FromBody] UserLoginModel userLogin)
        {
            User? currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == userLogin.Username);            
            if (currentUser != null)
            {
                if (currentUser.Password == userLogin.Password)
                {
                    var  token = _jwtWorker.GenerateToken(currentUser);
                    return Ok(token);
                }
                else
                {
                    return BadRequest($"Wrong Password");
                }
            }

            return BadRequest($"\"{userLogin.Username}\" not registered in the system. Your request does not handled");
        }

        [HttpPost]
        [Route("message")]
        public async Task<ActionResult<List<Message>>> PostMessage([FromBody] PostMessageModel dto)
        {
            User? currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (currentUser != null)
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
                        foreach (var message in range)
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
                //return Created("Message was added to Data Base. Text of message {}", dto.TextOfMessage);
                return Ok($"Message was added to Data Base. Text of message: {dto.TextOfMessage}");
            }
            else
            {
                return BadRequest($"\"{dto.Username}\" not registered in the system. Your request does not handled");
            }

        }


    }
}
