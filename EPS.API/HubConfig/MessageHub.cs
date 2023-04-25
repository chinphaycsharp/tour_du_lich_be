using Microsoft.AspNetCore.SignalR;

namespace EPS.API.HubConfig
{
    public class MessageHub : Hub
    {
        //private MessageService _messageService;
        private Data.EPSContext _context;
        //public static System.Collections.Generic.List<Message> messages;
        //public MessageHub(MessageService messageService,EPSContext context)
        //{
        //    _context = context;
        //}

        public async void SendMessage(string userName, string messageSend, int user_id, int isAdmin, int clientId)
        {
            //var strDate = DateTime.Now.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
            //message m = new message();
            //m.username = userName;
            //m.user_id = user_id;
            //m.isAdmin = isAdmin;
            //m.createdDate = DateTime.Now;
            //m.content = messageSend;
            //m.clientId = clientId;
            //await _context.message.AddAsync(m);
            //_context.SaveChanges();
            //await Clients.All.SendAsync("SendMessage", userName, messageSend, user_id, strDate, isAdmin, clientId);
            // await _messageService.SaveMessage(messageDto);
        }

        public void NewUser(string userName, string connectionId)
        {
            //var data = (from d in _context.message orderby d.createdDate select d).ToList();
            //Clients.Client(connectionId).SendAsync("PreviousMessage", data);
            //Clients.All.SendAsync("NewUser", userName);
        }
    }

    //public class MessageResult
    //{
    //    public List<message> messages { get; set; }
    //    public bool isAdmin { get; set; }
    //    public bool notification { get; set; }
    //}
}
