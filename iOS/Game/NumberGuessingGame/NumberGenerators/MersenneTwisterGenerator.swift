import Foundation

/// Mersenne Twister implementation
class MersenneTwisterGenerator: CustomRandomGenerator {
    private var mt: [UInt32]
    private var mti: Int
    
    /// Initializes the Mersenne Twister with a seed value
    /// - Parameter seed: The initial seed value (defaults to current timestamp)
    init(seed: UInt32 = UInt32(Date().timeIntervalSince1970)) {
        mt = Array(repeating: 0, count: 624)
        mti = 624
        mt[0] = seed
        
        for i in 1..<624 {
            mt[i] = UInt32(1812433253) &* (mt[i-1] ^ (mt[i-1] >> 30)) &+ UInt32(i)
        }
    }
    
    /// Generates the next batch of random numbers
    private func generateNumbers() {
        for i in 0..<624 {
            let y = (mt[i] & 0x80000000) | (mt[(i + 1) % 624] & 0x7fffffff)
            mt[i] = mt[(i + 34) % 624] ^ (y >> 1)
            if y % 2 != 0 {
                mt[i] ^= 0x9908b0df
            }
        }
        mti = 0
    }
    
    func nextInt(in range: ClosedRange<Int>) -> Int {
        if mti >= 624 {
            generateNumbers()
        }
        
        var y = mt[mti]
        mti += 1
        
        y ^= (y >> 11)
        y ^= (y << 7) & 0x9d2c5680
        y ^= (y << 15) & 0xefc60000
        y ^= (y >> 18)
        
        let randomValue = Double(y) / Double(UInt32.max)
        let rangeSize = range.upperBound - range.lowerBound + 1
        return range.lowerBound + Int(randomValue * Double(rangeSize))
    }
    
    func reset() {
        let seed = UInt32(Date().timeIntervalSince1970)
        mt = Array(repeating: 0, count: 624)
        mti = 624
        mt[0] = seed
        
        for i in 1..<624 {
            mt[i] = UInt32(1812433253) &* (mt[i-1] ^ (mt[i-1] >> 30)) &+ UInt32(i)
        }
    }
}
