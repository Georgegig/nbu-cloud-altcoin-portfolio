﻿using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioCommon.Access
{
    public class CloudQueueAccess
    {
        public void ClearDrawRaffleQueue()
        {
            CloudQueue queue = createQueueReference(Constants.QueueNames.GetPortfolio);
            if (queue.Exists())
            {
                queue.Clear();
            }
        }

        //// Queue name must be lowercase - Bad Request error is returned if queue with uppercase letter in the name is created
        private CloudQueue createQueueReference(string queueName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.DevelopmentStorageAccount;

            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference(queueName);

            return queue;
        }

        public CloudQueueMessage ReadGetPortfolioMessage()
        {
            CloudQueue queue = createQueueReference(Constants.QueueNames.GetPortfolio);

            if (!queue.Exists())
            {
                return null;
            }

            CloudQueueMessage retrievedMessage = queue.GetMessage();
            return retrievedMessage;
        }

        public void DeleteGetPortfolioMessage(CloudQueueMessage getPortfolioMessage)
        {
            CloudQueue queue = createQueueReference(Constants.QueueNames.GetPortfolio);

            if (!queue.Exists())
            {
                return;
            }

            try
            {
                queue.DeleteMessage(getPortfolioMessage);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Cannot delete message. " + ex.Message);
            }
        }

        //public void PostDrawRaffleMessage(Guid raffleId)
        //{
        //    CloudQueue queue = createQueueReference(Constants.QueueNames.DrawRaffle);

        //    queue.CreateIfNotExists();

        //    var message = new CloudQueueMessage(raffleId.ToString());
        //    queue.AddMessage(message);
        //}

    }
}
