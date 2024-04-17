import React, { useState } from 'react';
import axios from 'axios';

function Admin() {
    const [questions, setQuestions] = useState([]);

    const addQuestion = () => {
        setQuestions(prevQuestions => [...prevQuestions, {
            question: '',
            answers: ['', '', '', ''],
            general_feedback: '',
            specific_feedback: ['', '', '', ''],
            correct_answer: 1
        }]);
    };

    const handleQuestionChange = (index, event) => {
        const { name, value } = event.target;
        const newQuestions = [...questions];
        newQuestions[index][name] = value;
        setQuestions(newQuestions);
    };

    const handleAnswerChange = (questionIndex, answerIndex, event) => {
        const { value } = event.target;
        const newQuestions = [...questions];
        newQuestions[questionIndex].answers[answerIndex] = value;
        setQuestions(newQuestions);
    };

    const handleGeneralFeedbackChange = (index, event) => {
        const { value } = event.target;
        const newQuestions = [...questions];
        newQuestions[index].general_feedback = value;
        setQuestions(newQuestions);
    };

    const handleSpecificFeedbackChange = (questionIndex, feedbackIndex, event) => {
        const { value } = event.target;
        const newQuestions = [...questions];
        newQuestions[questionIndex].specific_feedback[feedbackIndex] = value;
        setQuestions(newQuestions);
    };

    const handleCorrectAnswerChange = (index, event) => {
        const { value } = event.target;
        const correctAnswer = parseInt(value, 10); // Parse value to integer
        const newQuestions = [...questions];
        newQuestions[index].correct_answer = correctAnswer;
        setQuestions(newQuestions);
        console.log(correctAnswer);
    };

    const uploadQuestions = () => {
        // Map questions to QuestionFeedbackDTO
        const questionFeedbackDTOList = questions.map((question, index) => ({
            question_num: index + 1,
            question: question.question,
            option1: question.answers[0],
            option2: question.answers[1],
            option3: question.answers[2],
            option4: question.answers[3],
            commonFeedback: question.general_feedback,
            feedback1: question.specific_feedback[0],
            feedback2: question.specific_feedback[1],
            feedback3: question.specific_feedback[2],
            feedback4: question.specific_feedback[3],
            correctAnswer: question.correct_answer
        }));

        // Make the POST request
        axios.post('http://localhost:8080/upload_questions', questionFeedbackDTOList)
            .then(response => {
                console.log('Success:', response);
                // Handle success response if needed
            })
            .catch(error => {
                console.error('Error:', error);
                // Handle error if needed
            });
    };

    return (
        <div style={{ height: '100vh', width: '97vw', overflow: 'auto', padding: '20px', position: 'relative' }}>
            {questions.map((question, index) => (
                <div key={index} style={{ border: '4px solid #ccc', padding: '10px', marginBottom: '10px', backgroundColor: "rgb(4, 34, 67)" }}>
                    <div style={{ marginBottom: '10px', color:"white" }}>Question {index + 1}</div>
                    <textarea 
                        type="text"
                        name="question"
                        placeholder="Enter question"
                        value={question.question}
                        onChange={(event) => handleQuestionChange(index, event)}
                        style={{ marginBottom: '10px', width: '99%', height: '30px', resize: 'none', backgroundColor: '#cdecfa' }}
                    />
                    {question.answers.map((answer, answerIndex) => (
                        <textarea
                            key={answerIndex}
                            type="text"
                            placeholder={`Enter answer ${answerIndex + 1}`}
                            value={answer}
                            onChange={(event) => handleAnswerChange(index, answerIndex, event)}
                            style={{ marginBottom: '10px', width: '99%', height: '15px', resize: 'none', backgroundColor: '#cdecfa' }}
                        />
                    ))}
                    <textarea
                        type="text"
                        placeholder="Enter general feedback"
                        value={question.general_feedback}
                        onChange={(event) => handleGeneralFeedbackChange(index, event)}
                        style={{ marginBottom: '10px', width: '99%', height: '30px', resize: 'none', backgroundColor: '#cdecfa' }}
                    />
                    {question.specific_feedback.map((feedback, feedbackIndex) => (
                        <textarea
                            key={feedbackIndex}
                            type="text"
                            placeholder={`Enter specific feedback ${feedbackIndex + 1}`}
                            value={feedback}
                            onChange={(event) => handleSpecificFeedbackChange(index, feedbackIndex, event)}
                            style={{ marginBottom: '10px', width: '99%', height: '30px', resize: 'none', backgroundColor: '#cdecfa' }}
                        />
                    ))}
                    <select value={question.correct_answer} onChange={(event) => handleCorrectAnswerChange(index, event)}>
                        <option value="1">Answer 1</option>
                        <option value="2">Answer 2</option>
                        <option value="3">Answer 3</option>
                        <option value="4">Answer 4</option>
                    </select>
                </div>
            ))}
            <button onClick={addQuestion} style={{ position: 'fixed', bottom: '20px', right: '220px', zIndex: '999', backgroundColor: "gray" }}>Add New Question</button>
            <button onClick={uploadQuestions} style={{ position: 'fixed', bottom: '20px', right: '20px', zIndex: '999', backgroundColor: "gray" }}>Upload Questions</button>
        </div>
    );
}

export default Admin;
