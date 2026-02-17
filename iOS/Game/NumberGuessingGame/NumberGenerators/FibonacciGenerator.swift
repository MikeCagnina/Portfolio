import Foundation

/// Fibonacci generator
class FibonacciGenerator: CustomRandomGenerator {
    private var values: [UInt64]
    private var currentIndex: Int
    
    /// Initializes the Fibonacci generator with a seed value
    /// - Parameter seed: The initial seed value (defaults to current timestamp)
    init(seed: UInt64 = UInt64(Date().timeIntervalSince1970)) {
        values = Array(repeating: 0, count: 55)
        currentIndex = 0
        
        // Initialize with better distributed values
        var currentSeed = seed
        for i in 0..<55 {
            values[i] = currentSeed
            // Use a simple LCG to generate next seed
            currentSeed = (currentSeed * 1103515245 + 12345) % 2147483648
        }
    }
    
    func nextInt(in range: ClosedRange<Int>) -> Int {
        let nextIndex = (currentIndex + 31) % 55
        let modulus: UInt64 = 2147483648  // Same as our LCG modulus
        values[currentIndex] = (values[currentIndex] + values[nextIndex]) % modulus
        
        let randomValue = Double(values[currentIndex]) / Double(modulus)
        currentIndex = (currentIndex + 1) % 55
        
        let rangeSize = range.upperBound - range.lowerBound + 1
        return range.lowerBound + Int(randomValue * Double(rangeSize))
    }
    
    func reset() {
        let seed = UInt64(Date().timeIntervalSince1970)
        values = Array(repeating: 0, count: 55)
        currentIndex = 0
        
        var currentSeed = seed
        for i in 0..<55 {
            values[i] = currentSeed
            currentSeed = (currentSeed * 1103515245 + 12345) % 2147483648
        }
    }
}
