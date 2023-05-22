using EPS.Data;
using EPS.Data.Entities;
using EPS.Service;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Globalization;
using System.Linq;

namespace EPS.API.HubConfig
{
    public class MessageHub : Hub
    {
        //private MessageService _messageService;
        private Data.EPSContext _context;
        //public static System.Collections.Generic.List<Message> messages;
        public MessageHub(EPSContext context)
        {
            _context = context;
        }

        public async void SendMessage(int id_tour, string messageSend, int starCount)
        {
            var strDate = DateTime.Now.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
            evaluate_tour m = new evaluate_tour();
            m.id_tour = id_tour;
            m.content = messageSend;
            m.star_count = starCount;
            m.created_time = DateTime.Now; 
            await _context.evaluate_tours.AddAsync(m);
            _context.SaveChanges();
            await Clients.All.SendAsync("SendMessage", id_tour, messageSend, starCount);
            // await _messageService.SaveMessage(messageDto);
        }

        public void GetMessage(int id_tour, string connectionId)
        {
            var data = _context.evaluate_tours.OrderBy(x=>x.created_time).Where(x=>x.id_tour == id_tour).Select(x=>x).ToList();
            Clients.Client(connectionId).SendAsync("PreviousMessage", data);
            Clients.All.SendAsync("GetMessage", id_tour);
        }
    }
}
