﻿/*
Application - The Multi server multi operating system control panel © copyright 2018.

Application is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License 3.0 as published by the Free Software Foundation.

This file is part of the Application project and is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License along with this file; if not, please contact Application at https://www.Application.com/contact

https://www.gnu.org/licenses/lgpl-3.0.txt
*/

using Application.Framework.Exceptions;
using Application.Framework.Utils;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Security.Cryptography;
using ECPoint = Org.BouncyCastle.Math.EC.ECPoint;

namespace Application.Framework
{
    public sealed class CryptoService : IDisposable, ICryptoService
    {
        private readonly DerObjectIdentifier _curve = SecObjectIdentifiers.SecP256r1;
        private SHA256 _sha256 = SHA256.Create();
        private RandomNumberGenerator _randomNumberGenerator;
        private const string ErrorDecodingPublicKey = "Error when decoding public key";
        private const string Sha256Exception = "Error when computing SHA-256";

        public CryptoService()
        {
            _sha256.Initialize();
            // TODO i should be able to get to the sha256 
            //_randomNumberGenerator = RandomNumberGenerator.Create("Sha256"); 
            _randomNumberGenerator = RandomNumberGenerator.Create();
        }

        public byte[] GenerateChallenge()
        {
            byte[] randomBytes = new byte[32];
            _randomNumberGenerator.GetBytes(randomBytes);

            return randomBytes;
        }

        public bool CheckSignature(X509Certificate attestationCertificate, byte[] signedBytes, byte[] signature)
        {
            return CheckSignature(attestationCertificate.GetPublicKey(), signedBytes, signature);
        }

        public ICipherParameters DecodePublicKey(byte[] encodedPublicKey)
        {
            try
            {
                X9ECParameters curve = SecNamedCurves.GetByOid(_curve);
                ECPoint point = curve.Curve.DecodePoint(encodedPublicKey);
                ECDomainParameters ecP = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);

                return new ECPublicKeyParameters(point, ecP);
            }
            catch (InvalidKeySpecException exception)
            {
                throw new U2fException(Resources.SignatureError, exception);
            }
            catch (Exception exception)
            {
                throw new U2fException(ErrorDecodingPublicKey, exception);
            }
        }

        public bool CheckSignature(ICipherParameters certificate, byte[] signedbytes, byte[] signature)
        {
            try
            {
                if (certificate == null || signedbytes == null || signedbytes.Length == 0
                    || signature == null || signature.Length == 0)
                    throw new ArgumentException(Resources.InvalidArguments);

                ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");
                signer.Init(false, certificate);
                signer.BlockUpdate(signedbytes, 0, signedbytes.Length);

                if (!signer.VerifySignature(signature))
                    throw new U2fException(Resources.SignatureError);

                return true;
            }
            catch (ArgumentException e)
            {
                throw e;
            }
            catch (InvalidKeySpecException e)
            {
                throw new U2fException(Resources.SignatureError, e);
            }
            catch (Exception e)
            {
                throw new U2fException(Resources.SignatureError, e);
            }
        }

        public byte[] Hash(string stringToHash)
        {
            return Hash(stringToHash.GetBytes());
        }

        public byte[] Hash(byte[] bytes)
        {
            try
            {
                byte[] hash = _sha256.ComputeHash(bytes);

                return hash;
            }
            catch (Exception exception)
            {
                throw new UnsupportedOperationException(Sha256Exception, exception);
            }
        }

        public void Dispose()
        {
            _sha256.Dispose();
            _sha256 = null;
            _randomNumberGenerator.Dispose();
            _randomNumberGenerator = null;
        }


        //      private readonly DerObjectIdentifier _curve = SecObjectIdentifiers.SecP256r1;
        //      private SHA256 _sha256 = SHA256.Create();
        //      private RandomNumberGenerator _randomNumberGenerator;

        //      private const string ErrorDecodingPublicKey = "Error when decoding public key";

        //      private const string Sha256Exception = "Error when computing SHA-256";

        //      public CryptoService()
        //      {
        //          _sha256.Initialize();
        //          // TODO i should be able to get to the sha256 
        //          //_randomNumberGenerator = RandomNumberGenerator.Create("Sha256"); 
        //          _randomNumberGenerator = RandomNumberGenerator.Create();
        //      }

        //      public byte[] GenerateChallenge()
        //      {
        //          byte[] randomBytes = new byte[32];
        //          _randomNumberGenerator.GetBytes(randomBytes);

        //          return randomBytes;
        //      }

        //public bool CheckSignature(X509Certificate attestationCertificate, byte[] signedBytes, byte[] signature)
        //      {
        //          return CheckSignature(attestationCertificate.GetPublicKey(), signedBytes, signature);
        //      }

        //      public ICipherParameters DecodePublicKey(byte[] encodedPublicKey)
        //      {
        //          try
        //          {
        //              X9ECParameters curve = SecNamedCurves.GetByOid(_curve);
        //              ECPoint point = curve.Curve.DecodePoint(encodedPublicKey);
        //              ECDomainParameters ecP = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);

        //              return new ECPublicKeyParameters(point, ecP);
        //          }
        //          catch (InvalidKeySpecException exception)
        //          {
        //              throw new U2fException(Resources.SignatureError, exception);
        //          }
        //          catch (Exception exception)
        //          {
        //              throw new U2fException(ErrorDecodingPublicKey, exception);
        //          }
        //      }
        //      public bool CheckSignature(ICipherParameters certificate, byte[] signedbytes, byte[] signature)
        //      {
        //          try
        //          {
        //              if (certificate == null || signedbytes == null || signedbytes.Length == 0
        //                  || signature == null || signature.Length == 0)
        //                  throw new ArgumentException(InvalidArgumentException);

        //              ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");
        //              signer.Init(false, certificate);
        //              signer.BlockUpdate(signedbytes, 0, signedbytes.Length);
        //              return true;
        //          }
        //          catch (Exception e)
        //          {
        //              throw e;
        //          }
        //      }

        //      public byte[] Hash(byte[] bytes)
        //      {
        //          try
        //          {
        //              byte[] hash = _sha256.ComputeHash(bytes);

        //              return hash;
        //          }
        //          catch (Exception exception)
        //          {
        //              throw exception;
        //          }
        //      }

        //      public void Dispose()
        //      {
        //          _sha256.Dispose();
        //          _sha256 = null;
        //          _randomNumberGenerator.Dispose();
        //          _randomNumberGenerator = null;
        //      }

        //      public bool CheckSignature(System.Security.Cryptography.X509Certificates.X509Certificate attestationCertificate, byte[] signedBytes, byte[] signature)
        //      {
        //          throw new NotImplementedException();
        //      }

        //      public ICipherParameters DecodePublicKey(byte[] encodedPublicKey)
        //      {
        //          throw new NotImplementedException();
        //      }

        //      public byte[] Hash(string stringToHash)
        //      {
        //          throw new NotImplementedException();
        //      }

        //      public bool CheckSignature(Org.BouncyCastle.X509.X509Certificate attestationCertificate, byte[] signedBytes, byte[] signature)
        //      {
        //          throw new NotImplementedException();
        //      }
    }
}
