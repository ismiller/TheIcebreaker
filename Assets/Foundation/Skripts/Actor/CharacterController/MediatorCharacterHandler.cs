using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class MediatorCharacterHandler : IMediator {
        
        private List<IMessageRecipients> recipients = new List<IMessageRecipients>();

        public MediatorCharacterHandler(List<BaseMainHandler> _recipients) {
            AddRecipient(_recipients);
        } 

        private void AddRecipient(List<BaseMainHandler> _recipients) {
            foreach (var recipient in _recipients) {
                if (recipient is IMessageRecipients) {
                    recipients.Add(recipient as IMessageRecipients);
                }
            }
        }

        public void Send<T>(T _mesage) {
            foreach (var recipient in recipients) {
                recipient.Notify<T>(_mesage);
            }
        }
    }    
}

