package com.example.questionare.DTO;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@AllArgsConstructor
@NoArgsConstructor
public class ResultDTO {
    private int question_num;
    private QuestionDTO questionDTO;
    private boolean status;
    private int correctAns;
    private String commonFeedback;
    private String specialFeedback;
}
