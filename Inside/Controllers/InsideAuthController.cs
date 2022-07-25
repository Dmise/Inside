﻿using Inside.Data;
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
    public class InsideAuthController : ControllerBase
    {
        private readonly InsideDbContext _context;
        private readonly JwtWorker _jwtWorker;
        

        public InsideAuthController (InsideDbContext context,
                             JwtWorker jwtWorker)
        {
            _context = context;
            _jwtWorker = jwtWorker;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]        
        public async Task<IActionResult> Login([FromBody] UserLoginModel userLogin)
        {
            if (userLogin.name == String.Empty || userLogin.password == String.Empty)
                throw new ArgumentOutOfRangeException();

            User? currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == userLogin.name);            
            if (currentUser != null)
            {
                if (currentUser.Password == userLogin.password)
                {
                    var  token = _jwtWorker.GenerateToken(currentUser);
                    return Ok(token);
                }
                else
                {
                    return BadRequest($"Wrong Password");
                }
            }

            return BadRequest($"\"{userLogin.name}\" not registered in the system. Your request does not handled");
        }

        [HttpPost]
        [Route("message")]
        public async Task<IActionResult> PostMessage([FromBody] PostMessageModel dto)
        {
            User? currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.name);
            if (currentUser != null)
            {
                if (dto.message == "history 10")
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
                    TextOfMessage = dto.message
                });
                await _context.SaveChangesAsync();
                //return Created("Message was added to Data Base. Text of message {}", dto.TextOfMessage);
                return Ok($"Message was added to Data Base. Text of message: {dto.message}");
            }
            else
            {
                return BadRequest($"\"{dto.name}\" not registered in the system. Your request does not handled");
            }

        }


    }
}
