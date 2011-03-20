using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FubuCore.Reflection;
using FubuCore.Util;
using FubuLocalization;

namespace FubuValidation
{
    public class Notification
    {
        // TODO -- does MessageBag deserve to live?
        private readonly Cache<Accessor, MessageBag> _bags;
        private readonly List<NotificationMessage> _messages = new List<NotificationMessage>();

        public Notification()
        {
            _bags = new Cache<Accessor, MessageBag>{
                OnMissing = (accessor => new MessageBag(accessor))
            };
        }

        public Notification(Type targetType)
            : this()
        {
            TargetType = targetType;
        }

        public Type TargetType { get; private set; }

        public NotificationMessage[] AllMessages
        {
            get { return _messages.ToArray(); }
        }

        public void Include(Notification notification)
        {
            _messages.AddRange(notification.AllMessages);
        }

        public void RegisterMessage(Accessor accessor, NotificationMessage notificationMessage)
        {
            notificationMessage.AddAccessor(accessor);

            if (!_messages.Contains(notificationMessage))
            {
                _messages.Add(notificationMessage);
                MessagesFor(accessor)
                    .Add(notificationMessage);
            }
        }

        public NotificationMessage RegisterMessage<T>(Expression<Func<T, object>> expression, StringToken token,
                                                      string messageTemplate)
        {
            return RegisterMessage(expression.ToAccessor(), token, messageTemplate);
        }

        public NotificationMessage RegisterMessage(Accessor accessor, StringToken token, string messageTemplate)
        {
            var notificationMessage = new NotificationMessage(token);
            RegisterMessage(accessor, notificationMessage);
            return notificationMessage;
        }

        public MessageBag MessagesFor(Accessor accessor)
        {
            return _bags[accessor];
        }

        public MessageBag MessagesFor<T>(Expression<Func<T, object>> property)
        {
            return MessagesFor(property.ToAccessor());
        }

        public void ForEachProperty(Action<MessageBag> action)
        {
            _bags.Each(action);
        }

        public bool IsValid()
        {
            return _messages.Count == 0;
        }

        public static Notification Valid()
        {
            return new Notification();
        }

        public NotificationMessage RegisterMessage(Accessor accessor, StringToken notificationMessage)
        {
            var message = new NotificationMessage(notificationMessage);
            RegisterMessage(accessor, message);

            return message;
        }
    }
}