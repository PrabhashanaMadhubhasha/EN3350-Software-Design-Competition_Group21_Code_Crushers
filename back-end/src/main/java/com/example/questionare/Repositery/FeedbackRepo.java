package com.example.questionare.Repositery;

import com.example.questionare.Entity.FeedBackEntity;
import com.example.questionare.Entity.QuestionEntity;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

public interface FeedbackRepo extends JpaRepository<FeedBackEntity, Integer> {
    @Query("SELECT f FROM FeedBackEntity f WHERE f.question_num = :questionNum")
    FeedBackEntity findFeedbackByQuestionNum(@Param("questionNum") int questionNum);
}
