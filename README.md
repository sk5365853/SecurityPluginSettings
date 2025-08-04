# Security Plugin Settings Application

## Description

This WPF app fetches 100 posts from the [JSONPlaceholder API](https://jsonplaceholder.typicode.com/posts) and displays them in a 10x10 grid. Each square shows the post's Id by default. Clicking any square toggles all squares to show the corresponding UserId instead, and vice versa.

## How to Run

1. Clone the repository.
2. Open the solution in Visual Studio 2022.
3. Restore NuGet packages (Newtonsoft.Json, Polly).
4. Build and run the project.
5. The main window will load and fetch data automatically.
6. Click on any square to toggle between Id and UserId for all squares.

## Gotchas

- API calls implement retry logic with Polly to handle transient network errors.
- The grid uses `UniformGrid` for consistent 10x10 layout.
- ObservableCollection is used for automatic UI updates.
- `RelayCommand` implementation is used for clean MVVM command binding.

## Motivation & Design Choices

- I chose the MVVM pattern for clear separation of concerns and testability.
- Polly retry policy ensures resiliency in case of temporary network issues.
- Using `ObservableCollection` ensures efficient UI updates without manual refresh.
- ToggleCommand updates a boolean property that controls the display mode, minimizing UI logic in code-behind.
- UniformGrid simplifies layout with fixed rows and columns without manual positioning.
- The UI prioritizes simplicity and responsiveness with clear visual feedback.

---

Feel free to extend with animations, custom styles, or further error handling!

