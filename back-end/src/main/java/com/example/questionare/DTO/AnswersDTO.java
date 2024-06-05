package com.example.questionare.DTO;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@AllArgsConstructor
@NoArgsConstructor
public class AnswersDTO { // Answer Class
    private int question_num;
    private int userIndex;
    private int answer;
}
