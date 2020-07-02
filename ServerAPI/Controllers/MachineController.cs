using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerAPI.Repositories;
using ClientMachineManager.Domain;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MachineController : ControllerBase
    {
        private readonly ILogger<MachineController> _logger;
        private NotificationsMessageHandler _notificationsMessageHandler { get; set; }

        public MachineController(ILogger<MachineController> logger,
                                 NotificationsMessageHandler notificationsMessageHandler)
        {
            _logger = logger;
            _notificationsMessageHandler = notificationsMessageHandler;
        }

        [HttpGet]
        public IEnumerable<MachineInfo> GetAll()
        {
            return MachinesRepository.GetAll();
        }

        [HttpPost("command/{ip}")]
        public async Task<CommandRunner> PostCommand(string ip, [FromBody]CommandRunner command)
        {
            CommandRunner cr = await _notificationsMessageHandler.SendMessageAsync(ip,command.ToJson());

            return cr;
        }
    }
}
