package com.example.questionare.DTO;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@AllArgsConstructor
@NoArgsConstructor
public class QuestionFeedbackDTO { // Feedback Class
    private int question_num;
    private String question;
    private String option1;
    private String option2;
    private String option3;
    private String option4;
    private String commonFeedback;
    private String feedback1;
    private String feedback2;
    private String feedback3;
    private String feedback4;
    private int correctAnswer;
}
