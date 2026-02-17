import SwiftUI

struct ContentView: View {
    @StateObject private var game = NumberGuessingGame()
    
    var body: some View {
        NavigationView {
            VStack(spacing: 20) {
                switch game.gameState {
                case .welcome:
                    welcomeView
                case .playing:
                    gameView
                case .won:
                    resultView
                case .lost:
                    resultView
                }
            }
            .padding()
            .navigationTitle("Number Guessing Game")
            .alert("Invalid Input", isPresented: $game.showAlert) {
                Button("OK") { }
            } message: {
                Text(game.alertMessage)
            }
        }
    }
    
    private var welcomeView: some View {
        VStack(spacing: 25) {
            Text("🎮")
                .font(.system(size: 80))
            
            Text("Welcome to the Number Guessing Game!")
                .font(.title)
                .fontWeight(.bold)
                .multilineTextAlignment(.center)
            
            Text("I'm thinking of a number between 1 and 100.\nYou have \(game.maxAttempts) attempts to guess it.")
                .font(.body)
                .multilineTextAlignment(.center)
                .foregroundColor(.secondary)
            
            VStack(spacing: 15) {
                Text("Choose Random Number Generator:")
                    .font(.headline)
                    .foregroundColor(.primary)
                
                Picker("Generator", selection: $game.selectedGenerator) {
                    ForEach(RandomGeneratorType.allCases, id: \.self) { generator in
                        VStack(alignment: .leading, spacing: 2) {
                            Text(generator.rawValue)
                                .font(.body)
                            Text(generator.description)
                                .font(.caption)
                                .foregroundColor(.secondary)
                        }
                        .tag(generator)
                    }
                }
                .pickerStyle(MenuPickerStyle())
                .padding(.horizontal)
                .padding(.vertical, 8)
                .background(Color.gray.opacity(0.1))
                .cornerRadius(8)
                .onChange(of: game.selectedGenerator) { _ in
                    game.updateGenerator()
                }
            }
            
            Button(action: {
                game.startNewGame()
            }) {
                Text("Start Game")
                    .font(.title2)
                    .fontWeight(.semibold)
                    .foregroundColor(.white)
                    .frame(maxWidth: .infinity)
                    .padding()
                    .background(Color.blue)
                    .cornerRadius(10)
            }
            
            // Generator Details Section
            VStack(alignment: .leading, spacing: 10) {
                Text("How \(game.selectedGenerator.rawValue) Works:")
                    .font(.headline)
                    .foregroundColor(.primary)
                
                ScrollView {
                    Text(game.selectedGenerator.detailedDescription)
                        .font(.body)
                        .foregroundColor(.secondary)
                        .multilineTextAlignment(.leading)
                        .fixedSize(horizontal: false, vertical: true)
                        .frame(maxWidth: .infinity, alignment: .leading)
                }
                .frame(maxHeight: 200)
                .padding()
                .background(Color.gray.opacity(0.05))
                .cornerRadius(8)
                .overlay(
                    RoundedRectangle(cornerRadius: 8)
                        .stroke(Color.gray.opacity(0.2), lineWidth: 1)
                )
            }
            .padding(.top, 10)
        }
    }
    
    private var gameView: some View {
        VStack(spacing: 20) {
            Text("🎯")
                .font(.system(size: 60))
            
            Text("Attempts: \(game.attempts)/\(game.maxAttempts)")
                .font(.headline)
                .foregroundColor(.secondary)
            
            if !game.message.isEmpty {
                Text(game.message)
                    .font(.body)
                    .padding()
                    .background(Color.gray.opacity(0.1))
                    .cornerRadius(8)
            }
            
            VStack(spacing: 20) {
                Text("Select your guess:")
                    .font(.headline)
                    .foregroundColor(.secondary)
                
                Picker("Number", selection: $game.selectedGuess) {
                    ForEach(1...100, id: \.self) { number in
                        Text("\(number)")
                            .tag(number)
                    }
                }
                .pickerStyle(WheelPickerStyle())
                .frame(height: 120)
                .clipped()
                .simultaneousGesture(
                    DragGesture()
                        .onChanged { _ in
                            // Wheel is being dragged/spun
                            game.setWheelSpinning(true)
                        }
                        .onEnded { _ in
                            // Wheel drag ended, but it might still be decelerating
                            DispatchQueue.main.asyncAfter(deadline: .now() + 0.3) {
                                game.setWheelSpinning(false)
                            }
                        }
                )
                .onChange(of: game.selectedGuess) { _ in
                    // Reset the timer whenever the value changes
                    game.setWheelSpinning(true)
                    DispatchQueue.main.asyncAfter(deadline: .now() + 0.3) {
                        game.setWheelSpinning(false)
                    }
                }
                
                Button(action: {
                    game.makeGuess()
                }) {
                    Text("Guess \(game.selectedGuess)")
                        .fontWeight(.semibold)
                        .foregroundColor(game.isWheelSpinning ? .gray : .white)
                        .frame(maxWidth: .infinity)
                        .padding()
                        .background(game.isWheelSpinning ? Color.gray.opacity(0.3) : Color.green)
                        .cornerRadius(10)
                        .overlay(
                            RoundedRectangle(cornerRadius: 10)
                                .stroke(game.isWheelSpinning ? Color.gray : Color.clear, lineWidth: 1)
                        )
                }
                .disabled(game.isWheelSpinning)
            }
            
            Spacer()
        }
    }
    
    private var resultView: some View {
        VStack(spacing: 30) {
            Text(game.gameState == .won ? "🎉" : "😔")
                .font(.system(size: 80))
            
            Text(game.message)
                .font(.title2)
                .fontWeight(.semibold)
                .multilineTextAlignment(.center)
            
            VStack(spacing: 15) {
                Text("Choose Random Number Generator:")
                    .font(.headline)
                    .foregroundColor(.primary)
                
                Picker("Generator", selection: $game.selectedGenerator) {
                    ForEach(RandomGeneratorType.allCases, id: \.self) { generator in
                        VStack(alignment: .leading, spacing: 2) {
                            Text(generator.rawValue)
                                .font(.body)
                            Text(generator.description)
                                .font(.caption)
                                .foregroundColor(.secondary)
                        }
                        .tag(generator)
                    }
                }
                .pickerStyle(MenuPickerStyle())
                .padding(.horizontal)
                .padding(.vertical, 8)
                .background(Color.gray.opacity(0.1))
                .cornerRadius(8)
                .onChange(of: game.selectedGenerator) { _ in
                    game.updateGenerator()
                }
            }
            
            Button(action: {
                game.startNewGame()
            }) {
                Text("Play Again")
                    .font(.title2)
                    .fontWeight(.semibold)
                    .foregroundColor(.white)
                    .frame(maxWidth: .infinity)
                    .padding()
                    .background(Color.blue)
                    .cornerRadius(10)
            }
            
            // Generator Details Section
            VStack(alignment: .leading, spacing: 10) {
                Text("How \(game.selectedGenerator.rawValue) Works:")
                    .font(.headline)
                    .foregroundColor(.primary)
                
                ScrollView {
                    Text(game.selectedGenerator.detailedDescription)
                        .font(.body)
                        .foregroundColor(.secondary)
                        .multilineTextAlignment(.leading)
                        .fixedSize(horizontal: false, vertical: true)
                        .frame(maxWidth: .infinity, alignment: .leading)
                }
                .frame(maxHeight: 200)
                .padding()
                .background(Color.gray.opacity(0.05))
                .cornerRadius(8)
                .overlay(
                    RoundedRectangle(cornerRadius: 8)
                        .stroke(Color.gray.opacity(0.2), lineWidth: 1)
                )
            }
            .padding(.top, 10)
        }
    }
}

#Preview {
    ContentView()
}
