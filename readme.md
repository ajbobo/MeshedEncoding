# Meshed Encoding

This is a little example of something I'm calling Meshed Encoding. It may have a real name, but I don't know it.
It will encode up to 64 characters into a 64 byte result. The result is always 64 bytes.
Each character in the incoming message is split into its 8 bits and spread across 8 bytes in the result.
This doesn't compress data or save any space, but it obfuscates data in an interesting way.

This is based on one of the encoding methods described in [this video](https://www.youtube.com/watch?v=8Da2fweYTkc) from The Modern Rogue
