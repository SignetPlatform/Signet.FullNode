using NBitcoin;

namespace Signet.Networks
{
    public static class SignetNetworks
    {
        public static NetworksSelector Signet
        {
            get
            {
                return new NetworksSelector(() => new SignetMain(), () => new SignetTest(), () => new SignetRegTest());
            }
        }
    }
}
