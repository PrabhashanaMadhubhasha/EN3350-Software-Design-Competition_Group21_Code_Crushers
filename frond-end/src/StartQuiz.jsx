import React, { useEffect } from 'react'; // Import useEffect
import backgroudVideo from './../videos/city.mp4';
import "./StartQuiz.css";
import { useParams, useNavigate } from 'react-router-dom';
import Typewriter from "typewriter-effect";
import image1 from './assets/game1.jpg'
import image2 from './assets/game3.jpg'
import image3 from './assets/game2.jpg'

function StartQuiz() {
    const { userIndex } = useParams();
    const Text  = "Hi, " + userIndex;

    const navigate = useNavigate();

    useEffect(() => {
        const timer = setTimeout(() => {
            navigate(`./../../quiz/${userIndex}`); 
        }, 9000); 

        return () => clearTimeout(timer);
    }, []); 

    return (
        <div className="video-background">
            <video autoPlay muted className='video'>
                <source src={backgroudVideo} type='video/mp4'/>
            </video>
            <div className="content">
                <div className='gameName'>
                    <h1>Modern City</h1>
                    <hr className='line1' />
                    <hr className='line2' />
                </div>
                <div className='hello'>
                <Typewriter
                onInit={(typewriter) => {
                    typewriter
                        .typeString(Text)
                        .pauseFor(600)
                        .deleteAll()
                        .typeString("Welcome to the questionare..!")
                        .pauseFor(600)
                        .start();
                }}
            />
                </div>
            </div>
            <div className='images'>
                <img src={image1} className="image" alt="Game 1" />
                <img src={image2} className="image" alt="Game 3" />
                <img src={image3} className="image" alt="Game 2" />
            </div>
        </div>
    );
}

export default StartQuiz;
