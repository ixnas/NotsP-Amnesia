function uint6ToB64 (nUint6) {

    return nUint6 < 26 ?
        nUint6 + 65
      : nUint6 < 52 ?
        nUint6 + 71
      : nUint6 < 62 ?
        nUint6 - 4
      : nUint6 === 62 ?
        43
      : nUint6 === 63 ?
        47
      :
        65;
  
  }
  
  export function base64EncArr (aBytes) {
  
    var eqLen = (3 - (aBytes.length % 3)) % 3, sB64Enc = "";
  
    for (var nMod3, nLen = aBytes.length, nUint24 = 0, nIdx = 0; nIdx < nLen; nIdx++) {
      nMod3 = nIdx % 3;
      /* Uncomment the following line in order to split the output in lines 76-character long: */
      /*
      if (nIdx > 0 && (nIdx * 4 / 3) % 76 === 0) { sB64Enc += "\r\n"; }
      */
      // eslint-disable-next-line
      nUint24 |= aBytes[nIdx] << (16 >>> nMod3 & 24);
      if (nMod3 === 2 || aBytes.length - nIdx === 1) {
        // eslint-disable-next-line
        sB64Enc += String.fromCharCode(uint6ToB64(nUint24 >>> 18 & 63), uint6ToB64(nUint24 >>> 12 & 63), uint6ToB64(nUint24 >>> 6 & 63), uint6ToB64(nUint24 & 63));
        nUint24 = 0;
      }
    }
  
    return  eqLen === 0 ?
        sB64Enc
      :
        sB64Enc.substring(0, sB64Enc.length - eqLen) + (eqLen === 1 ? "=" : "==");
  
  }