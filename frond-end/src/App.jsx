import React from 'react';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import StartQuiz from './StartQuiz';
import Quiz from './components/Quiz/quiz';
import Result from './components/Result/result';
import Admin from './components/Admin/admin';

function App() {
  return (
    <div>
      <BrowserRouter>
        <Routes>
          <Route path="/:userIndex" element={<StartQuiz />} />
          <Route path="/quiz/:userIndex" element={<Quiz />} />
          <Route path="/result/:userIndex" element={<Result />} />
          <Route path="/admin" element={<Admin />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
