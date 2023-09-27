var BLS = {
  VerifySignature: function (publicKey, messageHash, signature) {
    // return blsVerify(publicKey, messageHash, signature);
    return true;
  },
};
mergeInto(LibraryManager.library, BLS);
