package com.example.questionare.Repositery;


import com.example.questionare.Entity.QuestionEntity;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

public interface QuestionRepo extends JpaRepository<QuestionEntity, Integer> {
    @Query("SELECT q FROM QuestionEntity q WHERE q.question_num = :questionNum")
    QuestionEntity findQuestionByQuestionNum(@Param("questionNum") int questionNum);
}
