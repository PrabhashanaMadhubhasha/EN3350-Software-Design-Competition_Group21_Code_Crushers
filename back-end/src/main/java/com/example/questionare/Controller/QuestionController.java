package com.example.questionare.Controller;

import com.example.questionare.DTO.*;
import com.example.questionare.Service.QuestionareService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping
public class QuestionController {
    @Autowired
    private QuestionareService questionService;

    @CrossOrigin(origins = "http://localhost:5173")
    @GetMapping("/questions/{q_number}")
    public ResponseEntity<QuestionDTO> getQuestionByNumber(@PathVariable("q_number") int question_number) {
        QuestionDTO questionDTO = questionService.getQuestionByNumber(question_number);
        if (questionDTO != null) {
            return ResponseEntity.ok(questionDTO);
        } else {
            return ResponseEntity.notFound().build();
        }
    }

    @CrossOrigin(origins = "http://localhost:5173")
    @PostMapping("/upload_questions")
    public ResponseEntity<String> setQuestions(@RequestBody List<QuestionFeedbackDTO> questionFeedbackDTOList) {
        try {
            questionService.save_Questions_and_Feedbacks(questionFeedbackDTOList);
            return ResponseEntity.ok("Successful");
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body("Failed to upload questions.");
        }
    }

    @CrossOrigin(origins = "http://localhost:5173")
    @PostMapping("/updateAnswer")
    public ResponseEntity<String> setUserAnswers(@RequestBody AnswersDTO answersDTO){
        questionService.updateAnswer(answersDTO.getQuestion_num(), answersDTO.getUserIndex(), answersDTO.getAnswer());
        return ResponseEntity.ok("Successful");
    }

    @CrossOrigin(origins = "http://localhost:5173")
    @GetMapping("/result/{userIndex}")
    public List<ResultDTO> sendResult(@PathVariable("userIndex") int userIndex){
        return questionService.getResult(userIndex);
    }

    @CrossOrigin(origins = "http://localhost:5173")
    @GetMapping("/numberOfQuestions")
    public ResponseEntity<Integer> sendNumberOfQuestions(){
        return ResponseEntity.ok(questionService.numberOfQuestions());
    }

    @CrossOrigin(origins = "http://localhost:5173")
    @GetMapping("/getAnswer/{userIndex}/{questionNumber}")
    public ResponseEntity<AnswersDTO> sendAnswer(@PathVariable("userIndex") int userIndex, @PathVariable("questionNumber") int questionNumber){
        return ResponseEntity.ok(questionService.getUserAnswer(userIndex, questionNumber));
    }

}