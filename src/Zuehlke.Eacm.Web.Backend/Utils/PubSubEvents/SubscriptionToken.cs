using System;

namespace Zuehlke.Eacm.Web.Backend.Utils.PubSubEvents
{
    public class SubscriptionToken : IEquatable<SubscriptionToken>, IDisposable
    {
        private readonly Guid token;
        private Action<SubscriptionToken> unsubscribeAction;
        private bool disposed = false;

        public SubscriptionToken(Action<SubscriptionToken> unsubscribeAction)
        {
            this.unsubscribeAction = unsubscribeAction;
            this.token = Guid.NewGuid();
        }

        ~SubscriptionToken()
        {
            this.Dispose(false);
        }

        public bool Equals(SubscriptionToken other)
        {
            if (other == null)
            {
                return false;
            }

            return Equals(this.token, other.token);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return Equals(obj as SubscriptionToken);
        }

        public override int GetHashCode()
        {
            return this.token.GetHashCode();
        }

        #region IDisposable Support
        public void Dispose()
        {
            Dispose(true);         
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.unsubscribeAction != null)
                    {
                        this.unsubscribeAction(this);
                        this.unsubscribeAction = null;
                    }
                }

                this.disposed = true;
            }
        }
        #endregion
    }
}
