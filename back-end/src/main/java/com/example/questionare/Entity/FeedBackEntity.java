package com.example.questionare.Entity;

import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Entity
@Table(name = "feedbacks")
@Data
@AllArgsConstructor
@NoArgsConstructor
public class FeedBackEntity {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;
    private int question_num;
    private String commonFeedback;
    private String feedback1;
    private String feedback2;
    private String feedback3;
    private String feedback4;
    private int correct_answer;
}
