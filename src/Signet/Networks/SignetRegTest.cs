﻿//
//  Signet.FullNode - Blockchain / Cryptocurrency Platform
//  Portions Copyright © 2019 Synuit Software. All Rights Reserved.
//
//  $!!$ tac
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using NBitcoin;
using NBitcoin.BouncyCastle.Math;
using NBitcoin.Protocol;

namespace Signet.Networks
{
    public class SignetRegTest : SignetMain
    {
        public SignetRegTest()
        {
            this.Name = "SignetRegTest";
			this.NetworkType = NetworkType.Regtest;
			this.Magic = 0x43525901; // .CRT
            this.DefaultPort = 14333;
            this.DefaultRPCPort = 14334;
			this.DefaultAPIPort = 14335;
			this.MinTxFee = 0;
            this.FallbackFee = 0;
            this.MinRelayTxFee = 0;
            this.CoinTicker = "TSGT";
            this.DefaultBanTimeSeconds = 16000; // 500 (MaxReorg) * 64 (TargetSpacing) / 2 = 4 hours, 26 minutes and 40 seconds

            var powLimit = new Target(new uint256("7fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff"));

            var consensusFactory = new PosConsensusFactory();

            // Create the genesis block.
            this.GenesisTime = 1538568000; // 10/03/2018 @ 12:00pm (UTC)
            this.GenesisNonce = 82501;
            this.GenesisBits = 0x1F00FFFF;
            this.GenesisVersion = 1;
            this.GenesisReward = Money.Zero;

            // 2018-07-26: "We don’t need to fight the existing system, we just need to create a new one."
            // https://futurethinkers.org/vit-jedlicka-liberland/
            string pszTimestamp = "July 26, 2018, Future Thinkers, We don’t need to fight existing system, we create a new one";

            Block genesisBlock = CreateSignetGenesisBlock(pszTimestamp, consensusFactory, this.GenesisTime, this.GenesisNonce, this.GenesisBits, this.GenesisVersion, this.GenesisReward);

            //genesisBlock.Header.Time = 1494909211;
            //genesisBlock.Header.Nonce = 2433759;
            //genesisBlock.Header.Bits = powLimit;

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
                coinType: 1926,
                hashGenesisBlock: genesisBlock.GetHash(),
                subsidyHalvingInterval: 210000,
                majorityEnforceBlockUpgrade: 750,
                majorityRejectBlockOutdated: 950,
                majorityWindow: 1000,
                buriedDeployments: buriedDeployments,
                bip9Deployments: bip9Deployments,
                bip34Hash: new uint256("0x0000da5d40883d6c8aade797d8d6dcbf5cbc8e6428569170da39d2f01e8290e5"),
                ruleChangeActivationThreshold: 1916, // 95% of 2016
                minerConfirmationWindow: 2016, // nPowTargetTimespan / nPowTargetSpacing
                maxReorgLength: 500,
                defaultAssumeValid: null, // turn off assumevalid for regtest.
                maxMoney: long.MaxValue,
                coinbaseMaturity: 10,
                premineHeight: 2,
                premineReward: Money.Coins(13736000000),
                proofOfWorkReward: Money.Coins(2),
                powTargetTimespan: TimeSpan.FromSeconds(14 * 24 * 60 * 60), // two weeks
                powTargetSpacing: TimeSpan.FromSeconds(10 * 60),
                powAllowMinDifficultyBlocks: true,
				posNoRetargeting: false,
                powNoRetargeting: true,
                powLimit: powLimit,
                minimumChainWork: null,
                isProofOfStake: true,
                lastPowBlock: 125000,
                proofOfStakeLimit: new BigInteger(uint256.Parse("00000fffffffffffffffffffffffffffffffffffffffffffffffffffffffffff").ToBytes(false)),
                proofOfStakeLimitV2: new BigInteger(uint256.Parse("000000000000ffffffffffffffffffffffffffffffffffffffffffffffffffff").ToBytes(false)),
                proofOfStakeReward: Money.Coins(20)
            );

            this.Checkpoints = new Dictionary<int, CheckpointInfo>();

            this.Base58Prefixes[(int)Base58Type.PUBKEY_ADDRESS] = new byte[] { (66) };
            this.Base58Prefixes[(int)Base58Type.SCRIPT_ADDRESS] = new byte[] { (196) };
            this.Base58Prefixes[(int)Base58Type.SECRET_KEY] = new byte[] { (66 + 128) };

            this.DNSSeeds = new List<DNSSeedData>
            {
                new DNSSeedData("signet-chain.org", "regtestseed.city-chain.org"),
                new DNSSeedData("signet-coin.org", "regtestseed.city-coin.org"),
                new DNSSeedData("signetchain.foundation", "regtestseed.citychain.foundation")
            };

            this.SeedNodes = new List<NetworkAddress>
            {
                new NetworkAddress(IPAddress.Parse("13.73.143.193"), this.DefaultPort),
                new NetworkAddress(IPAddress.Parse("40.115.2.6"), this.DefaultPort),
                new NetworkAddress(IPAddress.Parse("13.66.158.6"), this.DefaultPort),
            };

            this.StandardScriptsRegistry = new SignetStandardScriptsRegistry();

            // 64 below should be changed to TargetSpacingSeconds when we move that field.
            Assert(this.DefaultBanTimeSeconds <= this.Consensus.MaxReorgLength * 64 / 2);
            Assert(this.Consensus.HashGenesisBlock == uint256.Parse("0x0000da5d40883d6c8aade797d8d6dcbf5cbc8e6428569170da39d2f01e8290e5"));
            Assert(this.Genesis.Header.HashMerkleRoot == uint256.Parse("0x49f8ad9e1d47aec09a38b7b54e282ed0ba30099b8632152931be74e2865266d5"));

            this.RegisterRules(this.Consensus);
        }
    }
}