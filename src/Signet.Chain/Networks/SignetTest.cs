//
//  Signet.FullNode - Blockchain / Cryptocurrency Platform
//  Portions Copyright © 2019 Synuit Software. All Rights Reserved.
//
//  $!!$ tac
//
using System;
using System.Collections.Generic;
using NBitcoin;
using NBitcoin.BouncyCastle.Math;
using NBitcoin.Protocol;

namespace Signet.Networks
{
    public class SignetTest : SignetMain
    {
        public SignetTest()
        {
            NetworkConfiguration config = new NetworkConfigurations().GetNetwork("testnet", "signet");
            //
            this.Name = "SignetTest";
            this.NetworkType = NetworkType.Testnet;
            this.Magic = 0x2e475453; // $!!$ tac  .GTS
            this.DefaultPort = config.Port;
            this.DefaultRPCPort = config.RpcPort;
            this.DefaultAPIPort = config.ApiPort;
            this.DefaultSignalRPort = config.WsPort;
            this.CoinTicker = "TSGT";
            this.DefaultBanTimeSeconds = 16000; // 500 (MaxReorg) * 64 (TargetSpacing) / 2 = 4 hours, 26 minutes and 40 seconds

            var powLimit = new Target(new uint256("0000ffff00000000000000000000000000000000000000000000000000000000"));

            var consensusFactory = new PosConsensusFactory();

            // Create the genesis block.
            this.GenesisTime = 1570492215;
            this.GenesisNonce = 11201;
            this.GenesisBits = 0x1F0FFFFF;
            this.GenesisVersion = 1;
            this.GenesisReward = Money.Zero;

            // SIGNET TEST GENESIS BLOCK:
            // 2019-09-24: "'Storm Area 51' has Nevada counties facing potentially hefty bills"
            // URL: https://www.foxnews.com/science/storm-area-51-nevada-counties-hefty-bills
            var pszTimestamp = "Sept. 24 2019, FoxNews.com, 'Storm Area 51' has Nevada counties facing ...";

            Block genesisBlock = CreateSignetGenesisBlock(pszTimestamp, consensusFactory, this.GenesisTime, this.GenesisNonce, this.GenesisBits, this.GenesisVersion, this.GenesisReward);

            this.Genesis = genesisBlock;

            var consensusOptions = new SignetPosConsensusOptions(
                maxBlockBaseSize: 1_000_000,
                maxStandardVersion: 2,
                maxStandardTxWeight: 100_000,
                maxBlockSigopsCost: 20_000,
                maxStandardTxSigopsCost: 20_000 / 5
            );

            var buriedDeployments = new BuriedDeploymentsArray
            {
                [BuriedDeployments.BIP34] = 0,
                [BuriedDeployments.BIP65] = 0,
                [BuriedDeployments.BIP66] = 0
            };

            var bip9Deployments = new SignetBIP9Deployments()
            {
                [SignetBIP9Deployments.ColdStaking] = new BIP9DeploymentsParameters("ColdStaking", 2,
                    new DateTime(2018, 12, 1, 0, 0, 0, DateTimeKind.Utc),
                    new DateTime(2019, 12, 1, 0, 0, 0, DateTimeKind.Utc))
            };

            this.Consensus = new Consensus(
                consensusFactory: consensusFactory,
                consensusOptions: consensusOptions,
                coinType: 1973, //$!!$ tac 1973
                hashGenesisBlock: genesisBlock.GetHash(),
                subsidyHalvingInterval: 210000,
                majorityEnforceBlockUpgrade: 750,
                majorityRejectBlockOutdated: 950,
                majorityWindow: 1000,
                buriedDeployments: buriedDeployments,
                bip9Deployments: bip9Deployments,
                bip34Hash: new uint256("0x0000000000000000000000000000000000000000000000000000000000000000"), // $!!$ tac
                ruleChangeActivationThreshold: 1916, // 95% of 2016
                minerConfirmationWindow: 2016, // nPowTargetTimespan / nPowTargetSpacing
                maxReorgLength: 500,
                defaultAssumeValid: new uint256("0x0000000000000000000000000000000000000000000000000000000000000000"), // $!!$ tac
                maxMoney: long.MaxValue,
                coinbaseMaturity: 10,
                premineHeight: 2,
                premineReward: Money.Coins(13736000000),
                proofOfWorkReward: Money.Coins(2),
                powTargetTimespan: TimeSpan.FromSeconds(14 * 24 * 60 * 60), // two weeks
                powTargetSpacing: TimeSpan.FromSeconds(10 * 60),
                powAllowMinDifficultyBlocks: false,
                posNoRetargeting: false,
                powNoRetargeting: false,
                powLimit: powLimit,
                minimumChainWork: null,
                isProofOfStake: true,
                lastPowBlock: 125000,
                proofOfStakeLimit: new BigInteger(uint256.Parse("00000fffffffffffffffffffffffffffffffffffffffffffffffffffffffffff").ToBytes(false)),
                proofOfStakeLimitV2: new BigInteger(uint256.Parse("000000000000ffffffffffffffffffffffffffffffffffffffffffffffffffff").ToBytes(false)),
                proofOfStakeReward: Money.Coins(20)
            );

            this.Base58Prefixes[(int)Base58Type.PUBKEY_ADDRESS] = new byte[] { (38) }; // G
            this.Base58Prefixes[(int)Base58Type.SCRIPT_ADDRESS] = new byte[] { (98) }; // g
            this.Base58Prefixes[(int)Base58Type.SECRET_KEY] = new byte[] { (38 + 128) }; // wif $!!$ ???

            this.Checkpoints = new Dictionary<int, CheckpointInfo>
            {
                //{ 0, new CheckpointInfo(new uint256("0x00077765f625cc2cb6266544ff7d5a462f25be14ea1116dc2bd2fec17e40a5e3"), new uint256("0x0000000000000000000000000000000000000000000000000000000000000000")) },
                //{ 2, new CheckpointInfo(new uint256("0xcf917ba726c8d05496a6b144fc433dc06cc574f49ca429e250454a0bbaab926d"), new uint256("0x315e64b6097a15128b0379c501ef278ff4fa70b062b44ce69a95e604464c46f8")) }, // Premine
                //{ 50, new CheckpointInfo(new uint256("0x464ae37e22cc44d0c8a86478ff95f98a6e1c44ceb3e175181e1a382270d1780c"), new uint256("0x6b5e010988dc9716e010e3ec21f7f60a4850726d5bbc7c212fab4e7b9b3566d7")) },
            };

            this.DNSSeeds = new List<DNSSeedData>
            {
                //new DNSSeedData("signet-chain.org", "testseed.city-chain.org"),
                //new DNSSeedData("signet-coin.org", "testseed.city-coin.org"),
                //new DNSSeedData("signetchain.foundation", "testseed.citychain.foundation")
            };

            this.SeedNodes = new List<NetworkAddress>
            {
                //new NetworkAddress(IPAddress.Parse("40.115.2.6"), this.DefaultPort),
                //new NetworkAddress(IPAddress.Parse("13.66.158.6"), this.DefaultPort),
                //new NetworkAddress(IPAddress.Parse("52.175.194.227"), this.DefaultPort)
            };

            this.StandardScriptsRegistry = new SignetStandardScriptsRegistry();

            // 64 below should be changed to TargetSpacingSeconds when we move that field.
            Assert(this.DefaultBanTimeSeconds <= this.Consensus.MaxReorgLength * 64 / 2);
            Assert(this.Consensus.HashGenesisBlock == uint256.Parse("0x000b40bcc84945ded1e1b98bcc235bf7a14d609e30adc69e394f13381596fb20"));
            Assert(this.Genesis.Header.HashMerkleRoot == uint256.Parse("0x84c08eccd31dfecf7072fca9697b4754b13709d787d32796dea399aeec18766c"));

            this.RegisterRules(this.Consensus);
        }
    }
}