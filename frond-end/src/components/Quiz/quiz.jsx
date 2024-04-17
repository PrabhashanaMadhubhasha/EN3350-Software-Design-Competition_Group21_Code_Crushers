import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import './quiz.css';
import axios from 'axios';
import background from '../../../videos/videoplayback.mp4'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faAngleLeft, faAngleRight } from '@fortawesome/free-solid-svg-icons';

const Quiz = () => {
    const [index, setIndex] = useState(1);
    const [question, setQuestion] = useState({});
    const [totalQuestions, setTotalQuestions] = useState(10);
    const { userIndex } = useParams();
    const [selectedAnswer, setSelectedAnswer] = useState(null);
    const navigate = useNavigate();

    const [borderStyles, setBorderStyles] = useState({
        option1: '2px solid #4bcb49',
        option2: '2px solid #4bcb49',
        option3: '2px solid #4bcb49',
        option4: '2px solid #4bcb49'
    });
  
    useEffect(() => {
        axios.get('http://localhost:8080/numberOfQuestions')
            .then(response => {
                setTotalQuestions(response.data);
            })
            .catch(error => {
                console.error('Error fetching total questions:', error);
            });
    }, []);

    useEffect(() => {
        fetchQuestionById(index);
    }, [index]); // Re-fetch question when index changes

    const fetchQuestionById = async (id) => {
        try {
            const questionResponse = await axios.get(`http://localhost:8080/questions/${id}`);
            setQuestion(questionResponse.data);
        } catch (error) {
            console.error(`Error fetching question ${id}:`, error);
        }
        try {
            const userAnswerResponce = await axios.get(`http://localhost:8080/getAnswer/${userIndex}/${id}`);
            const userAnswer = userAnswerResponce.data;
            setAns(userAnswer.answer);         
        } catch (error) {
            console.log("new user :", index);
        }
    };

    const handlePrevious = async () => {
        if (index > 1) {
            setIndex(index - 1);
        }
    };

    const handleNext = async () => {
        // Submit the answer to the backend
        const response = await axios.post('http://localhost:8080/updateAnswer', {
            question_num: index, // Use index as the question number
            userName: userIndex,
            answer: selectedAnswer // Pass the selected answer
        });
        // Go to next question
        if (index < totalQuestions) {
            setIndex(index + 1);
        }
        setSelectedAnswer(null);   
    };    

    const handleSubmit = async () => {
        const response = await axios.post('http://localhost:8080/updateAnswer', {
            question_num: index, // Use index as the question number
            userName: userIndex,
            answer: selectedAnswer // Pass the selected answer
        });
        setSelectedAnswer(null); 
        navigate(`./../../result/${userIndex}`);      
    };

    const setAns = (ans) => {
        setSelectedAnswer(ans);
        const updatedBorderStyles = {
            option1: '2px solid #4bcb49',
            option2: '2px solid #4bcb49',
            option3: '2px solid #4bcb49',
            option4: '2px solid #4bcb49'
        };
        updatedBorderStyles[`option${ans}`] = '2px solid rgb(233, 199, 10)';
        setBorderStyles(updatedBorderStyles);      
    };

    return (
        <div className='wrapper'>
            <video autoPlay loop muted className='video'>
                <source src={background} type='video/mp4' />
            </video>
            <div style={{ paddingRight: '80px', width: '160px' }}>
                {index > 0 && (
                    <button className='button' onClick={handlePrevious}>
                        <FontAwesomeIcon icon={faAngleLeft} size="2x" className="icon3" />
                        <FontAwesomeIcon icon={faAngleLeft} size="2x" className="icon2" />
                        <FontAwesomeIcon icon={faAngleLeft} size="2x" className="icon1" />
                    </button>
                )}
            </div>
            <div className='container'>
                <div style={{ height: '100px' }}>
                    <h2>{question.question}</h2>
                </div>
                <hr className='top_line' />
                <ul>
                    <li id="option1" onClick={() => { setAns(1) }} style={{ border: borderStyles.option1 }}>{question.option1}</li>
                    <li id="option2" onClick={() => { setAns(2) }} style={{ border: borderStyles.option2 }}>{question.option2}</li>
                    <li id="option3" onClick={() => { setAns(3) }} style={{ border: borderStyles.option3 }}>{question.option3}</li>
                    <li id="option4" onClick={() => { setAns(4) }} style={{ border: borderStyles.option4 }}>{question.option4}</li>
                </ul>
                <hr className='bottom_line' />
                <div className='index'>{index} of {totalQuestions} questions </div>
            </div>
            <div style={{ paddingLeft: '50px', width: '160px'}}>
                {index < totalQuestions && (
                    <button className='button' onClick={handleNext}>
                        <FontAwesomeIcon icon={faAngleRight} size="2x" className="icon1" />
                        <FontAwesomeIcon icon={faAngleRight} size="2x" className="icon2" />
                        <FontAwesomeIcon icon={faAngleRight} size="2x" className="icon3" />
                    </button>
                )}
                {index === totalQuestions && (
                    <button className='FilishButton' onClick={handleSubmit}> Finish </button>
                )}
            </div>
        </div>
    );
};

export default Quiz;
