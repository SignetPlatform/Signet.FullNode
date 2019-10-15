using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using NBitcoin;
using NBitcoin.BouncyCastle.Math;
using NBitcoin.DataEncoders;
using Signet.Networks;
using Xunit;

namespace Signet.Chain.Tests
{
    public class NetworkTests
    {
        [Fact]
        [Trait("UnitTest", "UnitTest")]
        public void CanGetNetworkFromName()
        {
            Network signetMain = new SignetMain();
            Network signetTestnet = new SignetTest();
            Network signetRegtest = new SignetRegTest();

            NetworkConfigurations netConfig = new NetworkConfigurations();

            Assert.Equal(netConfig.GetNetwork("mainnet", "signet").Port, signetMain.DefaultPort);
            Assert.Equal(netConfig.GetNetwork("mainnet", "signet").ApiPort, signetMain.DefaultAPIPort);
            Assert.Equal(netConfig.GetNetwork("mainnet", "signet").RpcPort, signetMain.DefaultRPCPort);
            Assert.Equal(netConfig.GetNetwork("mainnet", "signet").WsPort, signetMain.DefaultSignalRPort);

            Assert.Equal(netConfig.GetNetwork("testnet", "signet").Port, signetTestnet.DefaultPort);
            Assert.Equal(netConfig.GetNetwork("testnet", "signet").ApiPort, signetTestnet.DefaultAPIPort);
            Assert.Equal(netConfig.GetNetwork("testnet", "signet").RpcPort, signetTestnet.DefaultRPCPort);
            Assert.Equal(netConfig.GetNetwork("testnet", "signet").WsPort, signetTestnet.DefaultSignalRPort);

            Assert.Equal(netConfig.GetNetwork("regtest", "signet").Port, signetRegtest.DefaultPort);
            Assert.Equal(netConfig.GetNetwork("regtest", "signet").ApiPort, signetRegtest.DefaultAPIPort);
            Assert.Equal(netConfig.GetNetwork("regtest", "signet").RpcPort, signetRegtest.DefaultRPCPort);
            Assert.Equal(netConfig.GetNetwork("regtest", "signet").WsPort, signetRegtest.DefaultSignalRPort);

            Assert.Null(netConfig.GetNetwork("invalid", "signet"));
        }

        [Fact]
        [Trait("UnitTest", "UnitTest")]
        public void ReadSignetMainMagicByteWithFirstByteDuplicated()
        {
            var network = new SignetMain();
            List<byte> bytes = network.MagicBytes.ToList();
            bytes.Insert(0, bytes.First());

            using (var memstrema = new MemoryStream(bytes.ToArray()))
            {
                bool found = network.ReadMagic(memstrema, new CancellationToken());
                Assert.True(found);
            }
        }

        [Fact]
        [Trait("UnitTest", "UnitTest")]
        public void SignetMainIsInitializedCorrectly()
        {
            Network network = new SignetMain();

            //$!!$ EC: commenting out assertions until checkpoints, dns seeds, and seed nodes
            //$!!$ are set before launch
            //Assert.Equal(12, network.Checkpoints.Count);
            //Assert.Equal(4, network.DNSSeeds.Count);
            //Assert.Equal(4, network.SeedNodes.Count);

            Assert.Equal("SignetMain", network.Name);
            Assert.Equal(SignetMain.SignetRootFolderName, network.RootFolderName);
            Assert.Equal(SignetMain.SignetDefaultConfigFilename, network.DefaultConfigFilename);
            Assert.Equal(0x2e534754.ToString(), network.Magic.ToString());
            Assert.Equal(50505, network.DefaultPort);
            Assert.Equal(50506, network.DefaultRPCPort);
            Assert.Equal(50507, network.DefaultAPIPort);
            Assert.Equal(SignetMain.SignetMaxTimeOffsetSeconds, network.MaxTimeOffsetSeconds);
            Assert.Equal(SignetMain.SignetDefaultMaxTipAgeInSeconds, network.MaxTipAge);
            Assert.Equal(10000, network.MinTxFee);
            Assert.Equal(10000, network.FallbackFee);
            Assert.Equal(10000, network.MinRelayTxFee);
            Assert.Equal("SGT", network.CoinTicker);

            Assert.Equal(2, network.Bech32Encoders.Length);
            Assert.Equal(new Bech32Encoder("bc").ToString(), network.Bech32Encoders[(int)Bech32Type.WITNESS_PUBKEY_ADDRESS].ToString());
            Assert.Equal(new Bech32Encoder("bc").ToString(), network.Bech32Encoders[(int)Bech32Type.WITNESS_SCRIPT_ADDRESS].ToString());

            Assert.Equal(12, network.Base58Prefixes.Length);
            Assert.Equal(new byte[] { (63) }, network.Base58Prefixes[(int)Base58Type.PUBKEY_ADDRESS]);
            Assert.Equal(new byte[] { (125) }, network.Base58Prefixes[(int)Base58Type.SCRIPT_ADDRESS]);
            Assert.Equal(new byte[] { (63 + 128) }, network.Base58Prefixes[(int)Base58Type.SECRET_KEY]);
            Assert.Equal(new byte[] { 0x01, 0x42 }, network.Base58Prefixes[(int)Base58Type.ENCRYPTED_SECRET_KEY_NO_EC]);
            Assert.Equal(new byte[] { 0x01, 0x43 }, network.Base58Prefixes[(int)Base58Type.ENCRYPTED_SECRET_KEY_EC]);
            Assert.Equal(new byte[] { (0x04), (0x88), (0xB2), (0x1E) }, network.Base58Prefixes[(int)Base58Type.EXT_PUBLIC_KEY]);
            Assert.Equal(new byte[] { (0x04), (0x88), (0xAD), (0xE4) }, network.Base58Prefixes[(int)Base58Type.EXT_SECRET_KEY]);
            Assert.Equal(new byte[] { 0x2C, 0xE9, 0xB3, 0xE1, 0xFF, 0x39, 0xE2 }, network.Base58Prefixes[(int)Base58Type.PASSPHRASE_CODE]);
            Assert.Equal(new byte[] { 0x64, 0x3B, 0xF6, 0xA8, 0x9A }, network.Base58Prefixes[(int)Base58Type.CONFIRMATION_CODE]);
            Assert.Equal(new byte[] { 0x2a }, network.Base58Prefixes[(int)Base58Type.STEALTH_ADDRESS]);
            Assert.Equal(new byte[] { 23 }, network.Base58Prefixes[(int)Base58Type.ASSET_ID]);
            Assert.Equal(new byte[] { 0x13 }, network.Base58Prefixes[(int)Base58Type.COLORED_ADDRESS]);

            Assert.Equal(210000, network.Consensus.SubsidyHalvingInterval);
            Assert.Equal(750, network.Consensus.MajorityEnforceBlockUpgrade);
            Assert.Equal(950, network.Consensus.MajorityRejectBlockOutdated);
            Assert.Equal(1000, network.Consensus.MajorityWindow);
            Assert.Equal(0, network.Consensus.BuriedDeployments[BuriedDeployments.BIP34]);
            Assert.Equal(0, network.Consensus.BuriedDeployments[BuriedDeployments.BIP65]);
            Assert.Equal(0, network.Consensus.BuriedDeployments[BuriedDeployments.BIP66]);
            Assert.Equal(new uint256("0x0000000000000000000000000000000000000000000000000000000000000000"), network.Consensus.BIP34Hash);
            Assert.Equal(new Target(new uint256("00000fffffffffffffffffffffffffffffffffffffffffffffffffffffffffff")), network.Consensus.PowLimit);
            Assert.Null(network.Consensus.MinimumChainWork);
            Assert.Equal(TimeSpan.FromSeconds(14 * 24 * 60 * 60), network.Consensus.PowTargetTimespan);
            Assert.Equal(TimeSpan.FromSeconds(10 * 60), network.Consensus.PowTargetSpacing);
            Assert.False(network.Consensus.PowAllowMinDifficultyBlocks);
            Assert.False(network.Consensus.PowNoRetargeting);
            Assert.Equal(1916, network.Consensus.RuleChangeActivationThreshold);
            Assert.Equal(2016, network.Consensus.MinerConfirmationWindow);
            Assert.Null(network.Consensus.BIP9Deployments[SignetBIP9Deployments.TestDummy]);
            Assert.Equal(25000, network.Consensus.LastPOWBlock);
            Assert.True(network.Consensus.IsProofOfStake);
            Assert.Equal(1973, network.Consensus.CoinType);
            Assert.Equal(new BigInteger(uint256.Parse("00000fffffffffffffffffffffffffffffffffffffffffffffffffffffffffff").ToBytes(false)), network.Consensus.ProofOfStakeLimit);
            Assert.Equal(new BigInteger(uint256.Parse("000000000000ffffffffffffffffffffffffffffffffffffffffffffffffffff").ToBytes(false)), network.Consensus.ProofOfStakeLimitV2);
            Assert.Equal(new uint256("0x0000000000000000000000000000000000000000000000000000000000000000"), network.Consensus.DefaultAssumeValid);
            Assert.Equal(50, network.Consensus.CoinbaseMaturity);
            Assert.Equal(Money.Coins(98000000), network.Consensus.PremineReward);
            Assert.Equal(2, network.Consensus.PremineHeight);
            Assert.Equal(Money.Coins(4), network.Consensus.ProofOfWorkReward);
            Assert.Equal(Money.Coins(20), network.Consensus.ProofOfStakeReward);
            Assert.Equal((uint)500, network.Consensus.MaxReorgLength);
            Assert.Equal(long.MaxValue, network.Consensus.MaxMoney);

            Block genesis = network.GetGenesis();
            Assert.Equal(uint256.Parse("00000dfe58434aa9b9df28eb4b07453e0364992b50cfd1d6450c6abac82a02ed"), genesis.GetHash());
            Assert.Equal(uint256.Parse("0330848a5b9eee6d84984fb8e49110be9a7205b5746a2e1f8517f683a963558d"), genesis.Header.HashMerkleRoot);
        }

        //////////[Fact]
        //////////[Trait("UnitTest", "UnitTest")]
        //////////public void GenerateWitnessAddressAndVerify()
        //////////{
        //////////   List<Network> networks = new List<Network>();
        //////////   networks.Add(new SignetMain());
        //////////   networks.Add(new BitcoinMain());
        //////////   networks.Add(new StratisMain());

        //////////   foreach (Network network in networks)
        //////////   {
        //////////      var privateKey = new Key();
        //////////      BitcoinPubKeyAddress address = privateKey.PubKey.GetAddress(network);
        //////////      var witnessAddress = privateKey.PubKey.WitHash.ToString();
        //////////      var scriptPubKey = address.ScriptPubKey.ToString();
        //////////      //Assert.StartsWith("C", address.ToString());

        //////////      BitcoinSecret secret = privateKey.GetWif(network);
        //////////      var wif = secret.ToWif();
        //////////   }
        //////////}

        [Fact]
        [Trait("UnitTest", "UnitTest")]
        public void GenerateSomeSignetMainNetAddressAndVerifyPrefix()
        {
            Network network = new SignetMain();

            for (int i = 0; i < 10; i++)
            {
                var privateKey = new Key();
                BitcoinPubKeyAddress address = privateKey.PubKey.GetAddress(network);
                Assert.StartsWith("S", address.ToString());

                var witnessAddress = privateKey.PubKey.WitHash.ToString();
                var test = address.ScriptPubKey.ToString();

                BitcoinSecret secret = privateKey.GetWif(network);
                var wif = secret.ToWif();
                Assert.StartsWith("V", wif.ToString());
            }

            for (int i = 0; i < 10; i++)
            {
                var privateKey = new Key(false);
                BitcoinPubKeyAddress address = privateKey.PubKey.GetAddress(network);
                Assert.StartsWith("S", address.ToString());

                var witnessAddress = privateKey.PubKey.WitHash.ToString();
                var test = address.ScriptPubKey.ToString();

                BitcoinSecret secret = privateKey.GetWif(network);
                var wif = secret.ToWif();
                Assert.StartsWith("7", wif.ToString());
            }
        }

        [Fact]
        [Trait("UnitTest", "UnitTest")]
        public void ReadSignetTestMagicByteWithFirstByteDuplicated()
        {
            var network = new SignetTest();
            List<byte> bytes = network.MagicBytes.ToList();
            bytes.Insert(0, bytes.First());

            using (var memstrema = new MemoryStream(bytes.ToArray()))
            {
                bool found = network.ReadMagic(memstrema, new CancellationToken());
                Assert.True(found);
            }
        }

        [Fact]
        [Trait("UnitTest", "UnitTest")]
        public void SignetTestnetIsInitializedCorrectly()
        {
            Network network = new SignetTest();

            //$!!$ EC: commenting out assertions until checkpoints, dns seeds, and seed nodes
            //$!!$ are set before launch
            //Assert.Equal(3, network.Checkpoints.Count);
            //Assert.Equal(3, network.DNSSeeds.Count);
            //Assert.Equal(3, network.SeedNodes.Count);

            Assert.Equal("SignetTest", network.Name);
            Assert.Equal(SignetMain.SignetRootFolderName, network.RootFolderName);
            Assert.Equal(SignetMain.SignetDefaultConfigFilename, network.DefaultConfigFilename);
            Assert.Equal(0x2e475453.ToString(), network.Magic.ToString());
            Assert.Equal(60505, network.DefaultPort);
            Assert.Equal(60506, network.DefaultRPCPort);
            Assert.Equal(60507, network.DefaultAPIPort);
            Assert.Equal(SignetMain.SignetMaxTimeOffsetSeconds, network.MaxTimeOffsetSeconds);
            Assert.Equal(SignetMain.SignetDefaultMaxTipAgeInSeconds, network.MaxTipAge);
            Assert.Equal(10000, network.MinTxFee);
            Assert.Equal(10000, network.FallbackFee);
            Assert.Equal(10000, network.MinRelayTxFee);
            Assert.Equal("TSGT", network.CoinTicker);

            Assert.Equal(2, network.Bech32Encoders.Length);
            Assert.Equal(new Bech32Encoder("bc").ToString(), network.Bech32Encoders[(int)Bech32Type.WITNESS_PUBKEY_ADDRESS].ToString());
            Assert.Equal(new Bech32Encoder("bc").ToString(), network.Bech32Encoders[(int)Bech32Type.WITNESS_SCRIPT_ADDRESS].ToString());

            Assert.Equal(12, network.Base58Prefixes.Length);
            Assert.Equal(new byte[] { (38) }, network.Base58Prefixes[(int)Base58Type.PUBKEY_ADDRESS]);
            Assert.Equal(new byte[] { (98) }, network.Base58Prefixes[(int)Base58Type.SCRIPT_ADDRESS]);
            Assert.Equal(new byte[] { (38 + 128) }, network.Base58Prefixes[(int)Base58Type.SECRET_KEY]);
            Assert.Equal(new byte[] { 0x01, 0x42 }, network.Base58Prefixes[(int)Base58Type.ENCRYPTED_SECRET_KEY_NO_EC]);
            Assert.Equal(new byte[] { 0x01, 0x43 }, network.Base58Prefixes[(int)Base58Type.ENCRYPTED_SECRET_KEY_EC]);
            Assert.Equal(new byte[] { (0x04), (0x88), (0xB2), (0x1E) }, network.Base58Prefixes[(int)Base58Type.EXT_PUBLIC_KEY]);
            Assert.Equal(new byte[] { (0x04), (0x88), (0xAD), (0xE4) }, network.Base58Prefixes[(int)Base58Type.EXT_SECRET_KEY]);
            Assert.Equal(new byte[] { 0x2C, 0xE9, 0xB3, 0xE1, 0xFF, 0x39, 0xE2 }, network.Base58Prefixes[(int)Base58Type.PASSPHRASE_CODE]);
            Assert.Equal(new byte[] { 0x64, 0x3B, 0xF6, 0xA8, 0x9A }, network.Base58Prefixes[(int)Base58Type.CONFIRMATION_CODE]);
            Assert.Equal(new byte[] { 0x2a }, network.Base58Prefixes[(int)Base58Type.STEALTH_ADDRESS]);
            Assert.Equal(new byte[] { 23 }, network.Base58Prefixes[(int)Base58Type.ASSET_ID]);
            Assert.Equal(new byte[] { 0x13 }, network.Base58Prefixes[(int)Base58Type.COLORED_ADDRESS]);

            Assert.Equal(210000, network.Consensus.SubsidyHalvingInterval);
            Assert.Equal(750, network.Consensus.MajorityEnforceBlockUpgrade);
            Assert.Equal(950, network.Consensus.MajorityRejectBlockOutdated);
            Assert.Equal(1000, network.Consensus.MajorityWindow);
            Assert.Equal(0, network.Consensus.BuriedDeployments[BuriedDeployments.BIP34]);
            Assert.Equal(0, network.Consensus.BuriedDeployments[BuriedDeployments.BIP65]);
            Assert.Equal(0, network.Consensus.BuriedDeployments[BuriedDeployments.BIP66]);
            Assert.Equal(new uint256("0x0000000000000000000000000000000000000000000000000000000000000000"), network.Consensus.BIP34Hash);
            Assert.Equal(new Target(new uint256("0000ffff00000000000000000000000000000000000000000000000000000000")), network.Consensus.PowLimit);
            Assert.Null(network.Consensus.MinimumChainWork);
            Assert.Equal(TimeSpan.FromSeconds(14 * 24 * 60 * 60), network.Consensus.PowTargetTimespan);
            Assert.Equal(TimeSpan.FromSeconds(10 * 60), network.Consensus.PowTargetSpacing);
            Assert.False(network.Consensus.PowAllowMinDifficultyBlocks);
            Assert.False(network.Consensus.PowNoRetargeting);
            Assert.Equal(1916, network.Consensus.RuleChangeActivationThreshold);
            Assert.Equal(2016, network.Consensus.MinerConfirmationWindow);
            Assert.Null(network.Consensus.BIP9Deployments[SignetBIP9Deployments.TestDummy]);
            Assert.Equal(125000, network.Consensus.LastPOWBlock);
            Assert.True(network.Consensus.IsProofOfStake);
            Assert.Equal(1973, network.Consensus.CoinType);
            Assert.Equal(new BigInteger(uint256.Parse("00000fffffffffffffffffffffffffffffffffffffffffffffffffffffffffff").ToBytes(false)), network.Consensus.ProofOfStakeLimit);
            Assert.Equal(new BigInteger(uint256.Parse("000000000000ffffffffffffffffffffffffffffffffffffffffffffffffffff").ToBytes(false)), network.Consensus.ProofOfStakeLimitV2);
            Assert.Equal(new uint256("0x0000000000000000000000000000000000000000000000000000000000000000"), network.Consensus.DefaultAssumeValid);
            Assert.Equal(10, network.Consensus.CoinbaseMaturity);
            Assert.Equal(Money.Coins(13736000000), network.Consensus.PremineReward);
            Assert.Equal(2, network.Consensus.PremineHeight);
            Assert.Equal(Money.Coins(2), network.Consensus.ProofOfWorkReward);
            Assert.Equal(Money.Coins(20), network.Consensus.ProofOfStakeReward);
            Assert.Equal((uint)500, network.Consensus.MaxReorgLength);
            Assert.Equal(long.MaxValue, network.Consensus.MaxMoney);

            Block genesis = network.GetGenesis();
            Assert.Equal(uint256.Parse("000b40bcc84945ded1e1b98bcc235bf7a14d609e30adc69e394f13381596fb20"), genesis.GetHash());
            Assert.Equal(uint256.Parse("84c08eccd31dfecf7072fca9697b4754b13709d787d32796dea399aeec18766c"), genesis.Header.HashMerkleRoot);
        }

        [Fact]
        [Trait("UnitTest", "UnitTest")]
        public void GenerateSomeSignetTestNetAddressAndVerifyPrefix()
        {
            Network network = new SignetTest();

            for (int i = 0; i < 10; i++)
            {
                var privateKey = new Key();
                BitcoinPubKeyAddress address = privateKey.PubKey.GetAddress(network);
                Assert.StartsWith("G", address.ToString());

                var witnessAddress = privateKey.PubKey.WitHash.ToString();
                var test = address.ScriptPubKey.ToString();

                BitcoinSecret secret = privateKey.GetWif(network);
                var wif = secret.ToWif();
                Assert.StartsWith("R", wif.ToString());
            }

            for (int i = 0; i < 10; i++)
            {
                var privateKey = new Key(false);
                BitcoinPubKeyAddress address = privateKey.PubKey.GetAddress(network);
                Assert.StartsWith("G", address.ToString());

                var witnessAddress = privateKey.PubKey.WitHash.ToString();
                var test = address.ScriptPubKey.ToString();

                BitcoinSecret secret = privateKey.GetWif(network);
                var wif = secret.ToWif();
                Assert.StartsWith("6", wif.ToString());
            }
        }

        [Fact]
        [Trait("UnitTest", "UnitTest")]
        public void ReadSignetRegTestMagicByteWithFirstByteDuplicated()
        {
            var network = new SignetRegTest();
            List<byte> bytes = network.MagicBytes.ToList();
            bytes.Insert(0, bytes.First());

            using (var memstrema = new MemoryStream(bytes.ToArray()))
            {
                bool found = network.ReadMagic(memstrema, new CancellationToken());
                Assert.True(found);
            }
        }

        [Fact]
        [Trait("UnitTest", "UnitTest")]
        public void SignetRegTestIsInitializedCorrectly()
        {
            Network network = new SignetRegTest();

            Assert.Empty(network.Checkpoints);
            //$!!$ EC: commenting out assertions until checkpoints, dns seeds, and seed nodes
            //$!!$ are set before launch
            //Assert.Equal(3, network.DNSSeeds.Count);
            //Assert.Equal(3, network.SeedNodes.Count);

            Assert.Equal("SignetRegTest", network.Name);
            Assert.Equal(SignetMain.SignetRootFolderName, network.RootFolderName);
            Assert.Equal(SignetMain.SignetDefaultConfigFilename, network.DefaultConfigFilename);
            Assert.Equal(0x2e545347.ToString(), network.Magic.ToString());
            Assert.Equal(70505, network.DefaultPort);
            Assert.Equal(70506, network.DefaultRPCPort);
            Assert.Equal(70507, network.DefaultAPIPort);
            Assert.Equal(SignetMain.SignetMaxTimeOffsetSeconds, network.MaxTimeOffsetSeconds);
            Assert.Equal(SignetMain.SignetDefaultMaxTipAgeInSeconds, network.MaxTipAge);
            Assert.Equal(0, network.MinTxFee);
            Assert.Equal(0, network.FallbackFee);
            Assert.Equal(0, network.MinRelayTxFee);
            Assert.Equal("TSGT", network.CoinTicker);

            Assert.Equal(2, network.Bech32Encoders.Length);
            Assert.Equal(new Bech32Encoder("bc").ToString(), network.Bech32Encoders[(int)Bech32Type.WITNESS_PUBKEY_ADDRESS].ToString());
            Assert.Equal(new Bech32Encoder("bc").ToString(), network.Bech32Encoders[(int)Bech32Type.WITNESS_SCRIPT_ADDRESS].ToString());

            Assert.Equal(12, network.Base58Prefixes.Length);
            Assert.Equal(new byte[] { (65) }, network.Base58Prefixes[(int)Base58Type.PUBKEY_ADDRESS]);
            Assert.Equal(new byte[] { (127) }, network.Base58Prefixes[(int)Base58Type.SCRIPT_ADDRESS]);
            Assert.Equal(new byte[] { (65 + 128) }, network.Base58Prefixes[(int)Base58Type.SECRET_KEY]);
            Assert.Equal(new byte[] { 0x01, 0x42 }, network.Base58Prefixes[(int)Base58Type.ENCRYPTED_SECRET_KEY_NO_EC]);
            Assert.Equal(new byte[] { 0x01, 0x43 }, network.Base58Prefixes[(int)Base58Type.ENCRYPTED_SECRET_KEY_EC]);
            Assert.Equal(new byte[] { (0x04), (0x88), (0xB2), (0x1E) }, network.Base58Prefixes[(int)Base58Type.EXT_PUBLIC_KEY]);
            Assert.Equal(new byte[] { (0x04), (0x88), (0xAD), (0xE4) }, network.Base58Prefixes[(int)Base58Type.EXT_SECRET_KEY]);
            Assert.Equal(new byte[] { 0x2C, 0xE9, 0xB3, 0xE1, 0xFF, 0x39, 0xE2 }, network.Base58Prefixes[(int)Base58Type.PASSPHRASE_CODE]);
            Assert.Equal(new byte[] { 0x64, 0x3B, 0xF6, 0xA8, 0x9A }, network.Base58Prefixes[(int)Base58Type.CONFIRMATION_CODE]);
            Assert.Equal(new byte[] { 0x2a }, network.Base58Prefixes[(int)Base58Type.STEALTH_ADDRESS]);
            Assert.Equal(new byte[] { 23 }, network.Base58Prefixes[(int)Base58Type.ASSET_ID]);
            Assert.Equal(new byte[] { 0x13 }, network.Base58Prefixes[(int)Base58Type.COLORED_ADDRESS]);

            Assert.Equal(210000, network.Consensus.SubsidyHalvingInterval);
            Assert.Equal(750, network.Consensus.MajorityEnforceBlockUpgrade);
            Assert.Equal(950, network.Consensus.MajorityRejectBlockOutdated);
            Assert.Equal(1000, network.Consensus.MajorityWindow);
            Assert.Equal(0, network.Consensus.BuriedDeployments[BuriedDeployments.BIP34]);
            Assert.Equal(0, network.Consensus.BuriedDeployments[BuriedDeployments.BIP65]);
            Assert.Equal(0, network.Consensus.BuriedDeployments[BuriedDeployments.BIP66]);
            Assert.Equal(new uint256("0x0000000000000000000000000000000000000000000000000000000000000000"), network.Consensus.BIP34Hash);
            Assert.Equal(new Target(new uint256("7fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff")), network.Consensus.PowLimit);
            Assert.Null(network.Consensus.MinimumChainWork);
            Assert.Equal(TimeSpan.FromSeconds(14 * 24 * 60 * 60), network.Consensus.PowTargetTimespan);
            Assert.Equal(TimeSpan.FromSeconds(10 * 60), network.Consensus.PowTargetSpacing);
            Assert.True(network.Consensus.PowAllowMinDifficultyBlocks);
            Assert.True(network.Consensus.PowNoRetargeting);
            Assert.Equal(1916, network.Consensus.RuleChangeActivationThreshold);
            Assert.Equal(2016, network.Consensus.MinerConfirmationWindow);
            Assert.Null(network.Consensus.BIP9Deployments[SignetBIP9Deployments.TestDummy]);
            Assert.Equal(125000, network.Consensus.LastPOWBlock);
            Assert.True(network.Consensus.IsProofOfStake);
            Assert.Equal(1973, network.Consensus.CoinType);
            Assert.Equal(new BigInteger(uint256.Parse("00000fffffffffffffffffffffffffffffffffffffffffffffffffffffffffff").ToBytes(false)), network.Consensus.ProofOfStakeLimit);
            Assert.Equal(new BigInteger(uint256.Parse("000000000000ffffffffffffffffffffffffffffffffffffffffffffffffffff").ToBytes(false)), network.Consensus.ProofOfStakeLimitV2);
            Assert.Null(network.Consensus.DefaultAssumeValid);
            Assert.Equal(10, network.Consensus.CoinbaseMaturity);
            Assert.Equal(Money.Coins(13736000000), network.Consensus.PremineReward);
            Assert.Equal(2, network.Consensus.PremineHeight);
            Assert.Equal(Money.Coins(2), network.Consensus.ProofOfWorkReward);
            Assert.Equal(Money.Coins(20), network.Consensus.ProofOfStakeReward);
            Assert.Equal((uint)500, network.Consensus.MaxReorgLength);
            Assert.Equal(long.MaxValue, network.Consensus.MaxMoney);

            Block genesis = network.GetGenesis();
            Assert.Equal(uint256.Parse("0000c0b9eaf67a210b46218bad5d3587aab6500d4bc2b91b5b1a48c19ce8f6c7"), genesis.GetHash());
            Assert.Equal(uint256.Parse("a42c30d6615b79c0cc5abd6a9e72e4cf14b29fd4de7d14e62f0ce2bca9f196a8"), genesis.Header.HashMerkleRoot);
        }

        [Fact]
        [Trait("UnitTest", "UnitTest")]
        public void GenerateSomeSignetRegTestAddressAndVerifyPrefix()
        {
            Network network = new SignetRegTest();

            for (int i = 0; i < 10; i++)
            {
                var privateKey = new Key();
                BitcoinPubKeyAddress address = privateKey.PubKey.GetAddress(network);
                Assert.StartsWith("T", address.ToString());

                var witnessAddress = privateKey.PubKey.WitHash.ToString();
                var test = address.ScriptPubKey.ToString();

                BitcoinSecret secret = privateKey.GetWif(network);
                var wif = secret.ToWif();
                Assert.StartsWith("V", wif.ToString());
            }

            for (int i = 0; i < 10; i++)
            {
                var privateKey = new Key(false);
                BitcoinPubKeyAddress address = privateKey.PubKey.GetAddress(network);
                Assert.StartsWith("T", address.ToString());

                var witnessAddress = privateKey.PubKey.WitHash.ToString();
                var test = address.ScriptPubKey.ToString();

                BitcoinSecret secret = privateKey.GetWif(network);
                var wif = secret.ToWif();
                Assert.StartsWith("7", wif.ToString());
            }
        }
    }
}