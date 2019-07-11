﻿using Grpc.Core;
using LibraAdmissionControlClient.Dtos;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static AdmissionControl.AdmissionControl;
using Types;
using System;

namespace LibraAdmissionControlClient
{
    /// <summary>
    /// Main Class
    /// </summary>
    public class LibraAdmissionControlService
    {
        public ChannelState State
        {
            get
            {
                if (_channel == null)
                {
                    return ChannelState.Shutdown;
                }
                return _channel.State;
            }
        }

        private Channel _channel;
        public Channel Channel
        {
            get { return _channel; }
        }

        private AdmissionControlClient _client;
        public AdmissionControlClient Client
        {
            get { return _client; }
        }
      
        /// <summary>
        /// The Main method Initialize
        /// </summary>
        /// <param name="host">ac.testnet.libra.org</param>
        /// <param name="port">8000 or 30307</param>
        public LibraAdmissionControlService(string host, int port)
        {
            _channel = new Channel(host, port, ChannelCredentials.Insecure);
            _client = new AdmissionControlClient(_channel);
        }

        public async Task<Types.AccountStateWithProof> GetAccountInfoAsync(string address)
        {
            var updateToLatestLedgerRequest = new Types.UpdateToLatestLedgerRequest();
            var requestItem = new Types.RequestItem();
            var asr = new Types.GetAccountStateRequest();
            asr.Address = Google.Protobuf.ByteString.CopyFrom(address.StringToByteArray());
            requestItem.GetAccountStateRequest = asr;
            updateToLatestLedgerRequest.RequestedItems.Add(requestItem);

            var result = await _client.UpdateToLatestLedgerAsync(
                updateToLatestLedgerRequest, new Metadata());

            if (result == null ||
                result.ResponseItems == null ||
                result.ResponseItems != null && !result.ResponseItems.Any())
                throw new System.Exception("No such address.");

            var firest = result.ResponseItems.FirstOrDefault();

            if (firest.GetAccountStateResponse.AccountStateWithProof.Blob == null)
                throw new System.Exception("AccountStateWithProof Blob is empty.");

            return firest.GetAccountStateResponse.AccountStateWithProof;
        }

        public async Task<Types.TransactionListWithProof> GetTransactionsAsync(
            ulong startVersion, ulong limit)
        {
            var updateToLatestLedgerRequest = new Types.UpdateToLatestLedgerRequest();
            var requestItem = new Types.RequestItem();
            var tansactionRequest = new GetTransactionsRequest();
            requestItem.GetTransactionsRequest = tansactionRequest;
            tansactionRequest.StartVersion = startVersion;
            tansactionRequest.Limit = limit;
            tansactionRequest.FetchEvents = true;

            updateToLatestLedgerRequest.RequestedItems.Add(requestItem);
            var result = await _client.UpdateToLatestLedgerAsync(
                updateToLatestLedgerRequest, new Metadata());
            
            List<CustomRawTransaction> retList = new List<CustomRawTransaction>();
            foreach (var item in result.ResponseItems)
                return item.GetTransactionsResponse.TxnListWithProof;

            return null;
        }

        public async Task<Types.SignedTransactionWithProof> GetTransactionsBySequenceNumberAsync(
           string address, ulong sequenceNumber)
        {
            var updateToLatestLedgerRequest = new Types.UpdateToLatestLedgerRequest();
            var requestItem = new Types.RequestItem();
            var tansactionRequest = new GetAccountTransactionBySequenceNumberRequest();
            requestItem.GetAccountTransactionBySequenceNumberRequest = tansactionRequest;
            tansactionRequest.Account =
                Google.Protobuf.ByteString.CopyFrom(address.StringToByteArray());
            tansactionRequest.SequenceNumber = sequenceNumber;
            tansactionRequest.FetchEvents = true;

            updateToLatestLedgerRequest.RequestedItems.Add(requestItem);
            var result = await _client.UpdateToLatestLedgerAsync(
                updateToLatestLedgerRequest, new Metadata());

            foreach (var item in result.ResponseItems)
                return item.GetAccountTransactionBySequenceNumberResponse.SignedTransactionWithProof;

            return null;
        }

        public void Shutdown()
        {
            Channel.ShutdownAsync().Wait();
        }
    }
}