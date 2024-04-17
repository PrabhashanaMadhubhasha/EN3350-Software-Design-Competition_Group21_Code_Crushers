package com.example.questionare.Entity;

import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Entity
@Table(name = "users_answers")
@Data
@AllArgsConstructor
@NoArgsConstructor
public class AnswersEntity {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int Id;

    private int userIndex;
    private int question_num;
    private int userAnswer;

}


