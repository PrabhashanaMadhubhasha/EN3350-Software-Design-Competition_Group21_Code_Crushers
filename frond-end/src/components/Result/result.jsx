import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import './result.css'; 
import axios from 'axios';

const ResultPage = () => {
    const [results, setResults] = useState([]);
    const { userIndex } = useParams();

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await axios.get(`http://localhost:8080/result/${userIndex}`);
                setResults(response.data);
            } catch (error) {
                console.error('Error fetching data:', error);
            }
        };

        fetchData();
    }, [userIndex]);

    return (
        <div className="result-container">
            <div className="scrollable-feedback">
                {results.map((result, index) => (
                    <div className="result-item" key={index}>
                        <h2 className="question-num">Question {result.question_num}</h2>
                        <p className="question-text">{result.questionDTO.question}</p>
                        <p className={`${result.userAns !== 1 ? 'answer' : result.userAns === result.correctAns ? 'correct-answer' : 'wrong-answer'}`}>1. {result.questionDTO.option1}</p>
                        <p className={`${result.userAns !== 2 ? 'answer' : result.userAns === result.correctAns ? 'correct-answer' : 'wrong-answer'}`}>2. {result.questionDTO.option2}</p>
                        <p className={`${result.userAns !== 3 ? 'answer' : result.userAns === result.correctAns ? 'correct-answer' : 'wrong-answer'}`}>3. {result.questionDTO.option3}</p>
                        <p className={`${result.userAns !== 4 ? 'answer' : result.userAns === result.correctAns ? 'correct-answer' : 'wrong-answer'}`}>4. {result.questionDTO.option4}</p>
                        <p className="answer">Correct Answer: {result.correctAns}</p>
                        <div className="feedback-label">
                            <p className="feedback">Feedback 1: {result.commonFeedback}</p>
                            <p className="feedback">Feedback 2: {result.specialFeedback}</p>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default ResultPage;
