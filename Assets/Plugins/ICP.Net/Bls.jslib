var BLS = {
  VerifySignature: function (publicKeyHex, messageHashHex, signatureHex) {
    return nobleCurves.bls12_381.verify(publicKeyHex, messageHashHex, signatureHex);
  },
};
mergeInto(LibraryManager.library, BLS);
