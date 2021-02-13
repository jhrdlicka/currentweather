using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace currentweather.Hubs
{
    public class ServerUpdateHubMsg {
        public enum TOperation
        { 
            INSERT,
            UPDATE, 
            DELETE
        }
        public string entity; 
        public TOperation operation; 
        public string operationtxt;
        public long id;

        public ServerUpdateHubMsg(string pEntity, TOperation pOperation, long pId)
        {
            entity = pEntity;
            operation = pOperation;
            id = pId;

            TOperation lOperation=operation;
            operationtxt = lOperation switch
            {
                TOperation.INSERT => "insert",
                TOperation.UPDATE => "update",
                TOperation.DELETE => "delete",
                _ => "",
            };
            ;
        }

    }
    public class ServerUpdateHub : Hub
    {
        public async Task Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            await Clients.All.SendAsync("broadcastMessage", name, message);
        }

    }
}