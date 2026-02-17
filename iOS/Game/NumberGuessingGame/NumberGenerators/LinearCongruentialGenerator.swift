import Foundation

/// Linear Congruential Generator (LCG)
class LinearCongruentialGenerator: CustomRandomGenerator {
    private var seed: UInt64
    
    /// Initializes the LCG with a seed value
    /// - Parameter seed: The initial seed value (defaults to current timestamp)
    init(seed: UInt64 = UInt64(Date().timeIntervalSince1970)) {
        self.seed = seed
    }
    
    func nextInt(in range: ClosedRange<Int>) -> Int {
        // LCG parameters (using values from Numerical Recipes)
        let a: UInt64 = 1664525
        let c: UInt64 = 1013904223
        let m: UInt64 = 4294967296  // 2^32
        
        // Simple LCG formula: next = (a * current + c) % m
        seed = (a * seed + c) % m
        
        let randomValue = Double(seed) / Double(m)
        let rangeSize = range.upperBound - range.lowerBound + 1
        return range.lowerBound + Int(randomValue * Double(rangeSize))
    }
    
    func reset() {
        seed = UInt64(Date().timeIntervalSince1970)
    }
}
