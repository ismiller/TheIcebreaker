using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public interface IMediator {
        void Send<T>(T _mesage);
    }

    public interface IMessageRecipients {
        void Notify<T>(T _mesage);
    }
}
