﻿using System;
using Drone_Simulator.Browser;
using Drone_Simulator.Sockets;
using Xam.WebRtc.Android;

namespace Drone_Simulator.Signaling
{
    public class WebRtcSignalingInterface
    {
        private readonly ISocket _socket;

        public WebRtcSignalingInterface(ISocket _socket)
        {
            this._socket = _socket;
            this._socket.StringReceived += (type, message) =>
            {
                switch ((WebRtcMessageType)type)
                {
                    case WebRtcMessageType.Offer:
                        OfferReceived?.Invoke(message);
                        break;
                    case WebRtcMessageType.Answer:
                        AnswerReceived?.Invoke(message);
                        break;
                    case WebRtcMessageType.IceCandidate:
                        IceCandidateReceived?.Invoke(message);
                        break;
                }
            };
        }

        public event Action<string> OfferReceived;
        public event Action<string> AnswerReceived;
        public event Action<string> IceCandidateReceived;

        public void SendOffer(string offer)
        {
            _socket.SendString((sbyte)WebRtcMessageType.Offer, offer);
        }

        public void SendAnswer(string answer)
        {
            _socket.SendString((sbyte)WebRtcMessageType.Answer, answer);
        }

        public void SendIceCandidate(string iceCandidate)
        {
            _socket.SendString((sbyte)WebRtcMessageType.IceCandidate, iceCandidate);
        }
    }
}