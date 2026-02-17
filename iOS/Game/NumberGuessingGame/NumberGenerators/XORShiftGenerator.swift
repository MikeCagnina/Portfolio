import Foundation

/// XORShift generator
class XORShiftGenerator: CustomRandomGenerator {
    private var x: UInt64
    private var y: UInt64
    private var z: UInt64
    private var w: UInt64
    
    /// Initializes the XORShift generator with a seed value
    /// - Parameter seed: The initial seed value (defaults to current timestamp)
    init(seed: UInt64 = UInt64(Date().timeIntervalSince1970)) {
        // Ensure we have good initial values for XORShift
        self.x = seed
        self.y = seed ^ 0x9e3779b97f4a7c15
        self.z = (seed << 1) ^ 0xbf58476d1ce4e5b9
        self.w = (seed << 2) ^ 0x94d049bb133111eb
    }
    
    func nextInt(in range: ClosedRange<Int>) -> Int {
        let t = x ^ (x << 11)
        x = y
        y = z
        z = w
        w = w ^ (w >> 19) ^ t ^ (t >> 8)
        
        let randomValue = Double(w) / Double(UInt64.max)
        let rangeSize = range.upperBound - range.lowerBound + 1
        return range.lowerBound + Int(randomValue * Double(rangeSize))
    }
    
    func reset() {
        let seed = UInt64(Date().timeIntervalSince1970)
        x = seed
        y = seed ^ 0x9e3779b97f4a7c15
        z = (seed << 1) ^ 0xbf58476d1ce4e5b9
        w = (seed << 2) ^ 0x94d049bb133111eb
    }
}
