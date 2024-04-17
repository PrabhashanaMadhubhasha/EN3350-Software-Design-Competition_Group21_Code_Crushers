package com.example.questionare.Entity;

import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Entity
@Table(name = "questions")
@Data
@AllArgsConstructor
@NoArgsConstructor
public class QuestionEntity {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;
    private int question_num;
    private String question;
    private String option1;
    private String option2;
    private String option3;
    private String option4;

}
